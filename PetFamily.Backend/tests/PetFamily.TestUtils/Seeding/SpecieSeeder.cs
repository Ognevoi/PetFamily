using PetFamily.Application.Features.Species;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;
using PetFamily.Domain.SpecieManagement.Value_Objects;

namespace PetFamily.TestUtils.Seeding;

public static class SpecieSeeder
{
    public static async Task<Specie> SeedSpecieAsync(ISpeciesRepository context)
    {
        var specieId = SpecieId.NewSpecieId();
        var specie = Specie.Create(specieId, "Dog").Value;

        await context.Add(specie);
        await context.Save(specie);

        return specie;
    }

    public static async Task<Breed> SeedBreedAsync(ISpeciesRepository context, Specie specie)
    {
        var breedId = BreedId.NewBreedId();
        var breed = new Breed(breedId, "Labrador");

        specie.AddBreed(breed);
        await context.Save(specie);

        return breed;
    }
}