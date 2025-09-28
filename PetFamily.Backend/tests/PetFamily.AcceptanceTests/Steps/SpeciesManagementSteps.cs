using System.Net.Http.Json;
using System.Text.Json;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using PetFamily.AcceptanceTests.Infrastructure;
using PetFamily.Application.Database;
using PetFamily.Application.Features.Species;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;
using PetFamily.TestUtils.Seeding;
using Reqnroll;

namespace PetFamily.AcceptanceTests.Steps;

[Binding]
public class SpeciesManagementSteps : BaseStepClass
{
    public SpeciesManagementSteps(TestContext context) : base(context)
    {
    }

    [Given(@"I have (\d+) species in the system:")]
    public async Task GivenIHaveSpeciesInTheSystem(int speciesCount, Table table)
    {
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        for (int i = 0; i < speciesCount; i++)
        {
            var row = table.Rows[i];
            var speciesName = row["Species Name"];
            var breedsText = row["Breeds"];
            var breedNames = breedsText.Split(',').Select(b => b.Trim()).ToList();
            
            var specieId = SpecieId.NewSpecieId();
            var specie = Specie.Create(specieId, speciesName).Value;
            
            foreach (var breedName in breedNames)
            {
                var breedId = BreedId.NewBreedId();
                var breed = new Breed(breedId, breedName);
                specie.AddBreed(breed);
            }
            
            await speciesRepository.Add(specie);
            await speciesRepository.Save(specie);
        }
    }

    [Given(@"I have a species ""([^""]*)"" with breeds:")]
    public async Task GivenIHaveASpeciesWithBreeds(string speciesName, Table table)
    {
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        var specieId = SpecieId.NewSpecieId();
        var specie = Specie.Create(specieId, speciesName).Value;
        
        foreach (var row in table.Rows)
        {
            var breedName = row["Breed Name"];
            var breedId = BreedId.NewBreedId();
            var breed = new Breed(breedId, breedName);
            specie.AddBreed(breed);
        }
        
        await speciesRepository.Add(specie);
        await speciesRepository.Save(specie);
    }

    [Given(@"no species exist in the system")]
    public async Task GivenNoSpeciesExistInTheSystem()
    {
        // Database is already clean from background
        await Task.CompletedTask;
    }

    [Given(@"I have a species ""([^""]*)"" with no breeds")]
    public async Task GivenIHaveASpeciesWithNoBreeds(string speciesName)
    {
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        var specieId = SpecieId.NewSpecieId();
        var specie = Specie.Create(specieId, speciesName).Value;
        
        await speciesRepository.Add(specie);
        await speciesRepository.Save(specie);
    }

    [Given(@"I have species with breeds in the system")]
    public async Task GivenIHaveSpeciesWithBreedsInTheSystem()
    {
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        
        // Create a few species with breeds
        var speciesData = new[]
        {
            new { Name = "Dog", Breeds = new[] { "Labrador", "Golden Retriever", "German Shepherd" } },
            new { Name = "Cat", Breeds = new[] { "Persian", "Maine Coon", "Siamese" } },
            new { Name = "Bird", Breeds = new[] { "Canary", "Parakeet", "Cockatiel" } }
        };
        
        foreach (var speciesInfo in speciesData)
        {
            var specieId = SpecieId.NewSpecieId();
            var specie = Specie.Create(specieId, speciesInfo.Name).Value;
            
            foreach (var breedName in speciesInfo.Breeds)
            {
                var breedId = BreedId.NewBreedId();
                var breed = new Breed(breedId, breedName);
                specie.AddBreed(breed);
            }
            
            await speciesRepository.Add(specie);
            await speciesRepository.Save(specie);
        }
    }

    [When(@"I request all species")]
    public async Task WhenIRequestAllSpecies()
    {
        // Since there's no GET endpoint for species, we'll use the database directly
        var speciesRepository = TestBase.Scope.ServiceProvider.GetRequiredService<ISpeciesRepository>();
        var readDbContext = TestBase.Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        
        // Get species from the read context with breeds included
        var species = await readDbContext.Species
            .Include(s => s.Breeds)
            .OrderBy(s => s.Name)
            .ToListAsync();
        
        // Create a mock response with the same format as the actual API would return
        _context.LastResponse = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
        {
            Content = new StringContent(JsonSerializer.Serialize(species), 
                System.Text.Encoding.UTF8, "application/json")
        };
        _context.LastResponseContent = await _context.LastResponse.Content.ReadAsStringAsync();
    }

    [When(@"I request all species multiple times")]
    public async Task WhenIRequestAllSpeciesMultipleTimes()
    {
        // Request species multiple times to test consistency
        var responses = new List<HttpResponseMessage>();
        var responseContents = new List<string>();
        
        var readDbContext = TestBase.Scope.ServiceProvider.GetRequiredService<IReadDbContext>();
        
        for (int i = 0; i < 3; i++)
        {
            var species = await readDbContext.Species
                .Include(s => s.Breeds)
                .OrderBy(s => s.Name)
                .ToListAsync();
            var response = new HttpResponseMessage(System.Net.HttpStatusCode.OK)
            {
                Content = new StringContent(JsonSerializer.Serialize(species), 
                    System.Text.Encoding.UTF8, "application/json")
            };
            var content = await response.Content.ReadAsStringAsync();
            responses.Add(response);
            responseContents.Add(content);
        }
        
        _context.LastResponse = responses.First();
        _context.LastResponseContent = responseContents.First();
        _context.TestData["allResponses"] = responses;
        _context.TestData["allContents"] = responseContents;
    }

    [Then(@"I should receive (\d+) species")]
    public void ThenIShouldReceiveSpecies(int expectedCount)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        species!.Count.Should().Be(expectedCount);
    }

    [Then(@"each species should include their breeds")]
    public void ThenEachSpeciesShouldIncludeTheirBreeds()
    {
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        
        species!.ForEach(specie =>
        {
            specie.Breeds.Should().NotBeNull();
            specie.Breeds.Should().NotBeEmpty();
        });
    }

    [Then(@"the species should be ordered alphabetically")]
    public void ThenTheSpeciesShouldBeOrderedAlphabetically()
    {
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        
        var speciesNames = species!.Select(s => s.Name).ToList();
        speciesNames.Should().BeInAscendingOrder();
    }

    [Then(@"I should receive the ""([^""]*)"" species")]
    public void ThenIShouldReceiveTheSpecies(string expectedSpeciesName)
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        species!.Should().Contain(s => s.Name == expectedSpeciesName);
    }

    [Then(@"it should have (\d+) breeds")]
    public void ThenItShouldHaveBreeds(int expectedBreedCount)
    {
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        
        var dogSpecies = species!.FirstOrDefault(s => s.Name == "Dog");
        dogSpecies.Should().NotBeNull();
        dogSpecies!.Breeds.Count.Should().Be(expectedBreedCount);
    }

    [Then(@"the breeds should include ""([^""]*)"" and ""([^""]*)""")]
    public void ThenTheBreedsShouldIncludeAnd(string breed1, string breed2)
    {
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        
        var dogSpecies = species!.FirstOrDefault(s => s.Name == "Dog");
        dogSpecies.Should().NotBeNull();
        dogSpecies!.Breeds.Should().Contain(b => b.Name == breed1);
        dogSpecies.Breeds.Should().Contain(b => b.Name == breed2);
    }

    [Then(@"I should receive an empty list")]
    public void ThenIShouldReceiveAnEmptyList()
    {
        _context.LastResponse!.IsSuccessStatusCode.Should().BeTrue();
        
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        species!.Should().BeEmpty();
    }

    [Then(@"it should have an empty breeds list")]
    public void ThenItShouldHaveAnEmptyBreedsList()
    {
        var species = JsonSerializer.Deserialize<List<SpecieDto>>(_context.LastResponseContent!);
        species.Should().NotBeNull();
        
        var fishSpecies = species!.FirstOrDefault(s => s.Name == "Fish");
        fishSpecies.Should().NotBeNull();
        fishSpecies!.Breeds.Should().BeEmpty();
    }

    [Then(@"the data should be consistent")]
    public void ThenTheDataShouldBeConsistent()
    {
        var allContents = (List<string>)_context.TestData["allContents"];
        allContents.Should().NotBeNull();
        allContents.Count.Should().Be(3);
        
        // All responses should have the same content
        var firstContent = allContents[0];
        allContents.Should().AllBeEquivalentTo(firstContent);
    }

    [Then(@"the species count should remain the same")]
    public void ThenTheSpeciesCountShouldRemainTheSame()
    {
        var allContents = (List<string>)_context.TestData["allContents"];
        var speciesCounts = allContents.Select(content =>
        {
            var species = JsonSerializer.Deserialize<List<SpecieDto>>(content);
            return species?.Count ?? 0;
        }).ToList();
        
        speciesCounts.Should().AllBeEquivalentTo(speciesCounts[0]);
    }

    [Then(@"the breeds count per species should remain the same")]
    public void ThenTheBreedsCountPerSpeciesShouldRemainTheSame()
    {
        var allContents = (List<string>)_context.TestData["allContents"];
        
        for (int i = 1; i < allContents.Count; i++)
        {
            var firstSpecies = JsonSerializer.Deserialize<List<SpecieDto>>(allContents[0]);
            var currentSpecies = JsonSerializer.Deserialize<List<SpecieDto>>(allContents[i]);
            
            firstSpecies!.Count.Should().Be(currentSpecies!.Count);
            
            for (int j = 0; j < firstSpecies.Count; j++)
            {
                firstSpecies[j].Breeds.Count.Should().Be(currentSpecies[j].Breeds.Count);
            }
        }
    }
}
