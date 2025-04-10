using FluentAssertions;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.SpecieManagement.AggregateRoot;
using PetFamily.Domain.SpecieManagement.Entities;

namespace PetFamily.UnitTests;

public class VolunteerTests
{
    [Fact]
    public void Create_WithValidInputs_ShouldReturnSuccessResult()
    {
        // Arrange
        var volunteerId = VolunteerId.Create(Guid.NewGuid());
        var fullName = FullName.Create("Oleg", "Ognevoi").Value;
        var email = Email.Create("oleg.ognevoi@example.com").Value;
        var description = Description.Create("Experienced volunteer").Value;
        var experienceYears = ExperienceYears.Create(5).Value;
        var phoneNumber = PhoneNumber.Create("+12345678901").Value;

        // Act
        var result = Volunteer.Create(
            volunteerId,
            fullName,
            email,
            description,
            experienceYears,
            phoneNumber);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Id.Should().Be(volunteerId);
        result.Value.FullName.Should().Be(fullName);
        result.Value.Email.Should().Be(email);
        result.Value.Description.Should().Be(description);
        result.Value.ExperienceYears.Should().Be(experienceYears);
        result.Value.PhoneNumber.Should().Be(phoneNumber);
    }

    [Fact]
    public void Update_ShouldUpdateVolunteerProperties()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var newFullName = FullName.Create("Alice", "White").Value;
        var newEmail = Email.Create("alice.white@example.com").Value;
        var newDescription = Description.Create("Updated description").Value;
        var newExperienceYears = ExperienceYears.Create(8).Value;
        var newPhoneNumber = PhoneNumber.Create("+37255572551").Value;

        // Act
        volunteer.Update(
            newFullName,
            newEmail,
            newDescription,
            newExperienceYears,
            newPhoneNumber);

        // Assert
        volunteer.FullName.Should().Be(newFullName);
        volunteer.Email.Should().Be(newEmail);
        volunteer.Description.Should().Be(newDescription);
        volunteer.ExperienceYears.Should().Be(newExperienceYears);
        volunteer.PhoneNumber.Should().Be(newPhoneNumber);
    }

    [Fact]
    public void AddPet_ShouldAddPetToVolunteer()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();

        // Act
        var result = volunteer.AddPet(pet);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().Contain(pet);
        pet.Position.Value.Should().Be(1);
    }

    [Fact]
    public void GetPetById_WithExistingPet_ShouldReturnPet()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        volunteer.AddPet(pet);

        // Act
        var result = volunteer.GetPetById(pet.Id);

        // Assert
        result.IsSuccess.Should().BeTrue();
        result.Value.Should().Be(pet);
    }

    [Fact]
    public void GetPetById_WithNonExistingPet_ShouldReturnFailure()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var nonExistingPetId = PetId.NewPetId();

        // Act
        var result = volunteer.GetPetById(nonExistingPetId);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    [Fact]
    public void SoftDelete_ShouldMarkVolunteerAndPetsAsDeleted()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        volunteer.AddPet(pet);

        // Act
        volunteer.SoftDelete();

        // Assert
        volunteer.IsDeleted.Should().BeTrue();
        pet.IsDeleted.Should().BeTrue();
    }

    [Fact]
    public void Restore_ShouldRestoreVolunteerAndPets()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        volunteer.AddPet(pet);
        volunteer.SoftDelete();

        // Act
        volunteer.Restore();

        // Assert
        volunteer.IsDeleted.Should().BeFalse();
        pet.IsDeleted.Should().BeFalse();
    }

    [Fact]
    public void RemovePet_WithNonExistingPet_ShouldReturnFailure()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        var nonExistingPet = CreateTestPet();

        volunteer.AddPet(pet);

        // Act
        var result = volunteer.RemovePet(nonExistingPet);

        // Assert
        result.IsFailure.Should().BeTrue();
    }

    #region PetPositionTests

    [Fact]
    public void MovePet_ToSamePosition_ShouldReturnSuccess()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        volunteer.AddPet(pet);
        var initialPosition = pet.Position;

        // Act
        var result = volunteer.MovePet(pet, initialPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Should().Be(initialPosition);
    }

    [Fact]
    public void MovePet_WithOnlyOnePet_ShouldReturnSuccess()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet = CreateTestPet();
        volunteer.AddPet(pet);
        var initialPosition = pet.Position;
        var newPosition = Position.Create(2).Value;

        // Act
        var result = volunteer.MovePet(pet, newPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet.Position.Should().Be(initialPosition);
    }

    [Fact]
    public void MovePet_ToHigherPosition_ShouldReorderPetsCorrectly()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();

        volunteer.AddPet(pet1); // Position 1
        volunteer.AddPet(pet2); // Position 2
        volunteer.AddPet(pet3); // Position 3

        // Act
        var newPosition = Position.Create(3).Value;
        var result = volunteer.MovePet(pet1, newPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(3);
        pet2.Position.Value.Should().Be(1);
        pet3.Position.Value.Should().Be(2);
    }

    [Fact]
    public void MovePet_ToLowerPosition_ShouldReorderPetsCorrectly()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();

        volunteer.AddPet(pet1); // Position 1
        volunteer.AddPet(pet2); // Position 2
        volunteer.AddPet(pet3); // Position 3

        // Act
        var newPosition = Position.Create(1).Value;
        var result = volunteer.MovePet(pet3, newPosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(2);
        pet2.Position.Value.Should().Be(3);
        pet3.Position.Value.Should().Be(1);
    }

    [Fact]
    public void MovePet_ToPositionOutOfRange_ShouldAdjustPositionAndReorderCorrectly()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();

        volunteer.AddPet(pet1); // Position 1
        volunteer.AddPet(pet2); // Position 2

        // Act
        var outOfRangePosition = Position.Create(5).Value;
        var result = volunteer.MovePet(pet1, outOfRangePosition);

        // Assert
        result.IsSuccess.Should().BeTrue();
        pet1.Position.Value.Should().Be(2);
        pet2.Position.Value.Should().Be(1);
    }
    
    [Fact]
    public void RemovePet_WithExistingPet_ShouldRemovePetAndReorderPositions()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();
        var pet3 = CreateTestPet();

        volunteer.AddPet(pet1); // Position 1
        volunteer.AddPet(pet2); // Position 2
        volunteer.AddPet(pet3); // Position 3

        // Act
        var result = volunteer.RemovePet(pet2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().NotContain(pet2);
        pet1.Position.Value.Should().Be(1);
        pet3.Position.Value.Should().Be(2);
    }
    
    [Fact]
    public void RemovePet_InLastPosition_ShouldRemovePetAndNotReorderPositions()
    {
        // Arrange
        var volunteer = CreateTestVolunteer();
        var pet1 = CreateTestPet();
        var pet2 = CreateTestPet();

        volunteer.AddPet(pet1); // Position 1
        volunteer.AddPet(pet2); // Position 2

        // Act
        var result = volunteer.RemovePet(pet2);

        // Assert
        result.IsSuccess.Should().BeTrue();
        volunteer.Pets.Should().NotContain(pet2);
        pet1.Position.Value.Should().Be(1);
    }

    #endregion

    private static Volunteer CreateTestVolunteer()
    {
        var volunteerId = VolunteerId.Create(Guid.NewGuid());
        var fullName = FullName.Create("Oleg", "Ognevoi").Value;
        var email = Email.Create("oleg.ognevoi@example.com").Value;
        var description = Description.Create("Experienced volunteer").Value;
        var experienceYears = ExperienceYears.Create(5).Value;
        var phoneNumber = PhoneNumber.Create("+12345678901").Value;

        return Volunteer.Create(
            volunteerId,
            fullName,
            email,
            description,
            experienceYears,
            phoneNumber).Value;
    }

    private static Pet CreateTestPet()
    {
        var petId = PetId.NewPetId();
        var name = PetName.Create("Buddy").Value;
        var species = CreateTestSpecieWithBreed();
        var breed = species.Breed.First();
        var address = Address.Create("Valge 10", "Tallinn", "Harjumaa", "11413").Value;
        var description = Description.Create("Friendly pet").Value;
        var color = PetColor.Create("Black").Value;
        var healthInfo = PetHealthInfo.Create("Healthy").Value;
        var weight = Weight.Create(10).Value;
        var height = Height.Create(50).Value;
        var isSterilized = IsSterilized.Create(true).Value;
        var isVaccinated = IsVaccinated.Create(true).Value;
        var birthDate = DateTime.UtcNow.AddYears(-2);
        var status = PetStatus.NeedsHelp;

        return new Pet(
            petId,
            name,
            species,
            breed,
            address,
            description,
            color,
            healthInfo,
            weight,
            height,
            isSterilized,
            isVaccinated,
            birthDate,
            status);
    }

    private static Specie CreateTestSpecieWithBreed()
    {
        var id = Guid.NewGuid();
        var name = "Dog";
        var breed = new Breed(Guid.NewGuid(), "Labrador");

        var specie = Specie.Create(id, name);

        specie.Value.AddBreed(breed);

        return specie.Value;
    }
}