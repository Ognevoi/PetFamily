using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.AnimalSpecies.Entities;
using PetFamily.Domain.AnimalSpecies.Value_Objects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;


public class BreedConfiguration : IEntityTypeConfiguration<Breed>
{
    public void Configure(EntityTypeBuilder<Breed> builder)
    {
        builder.ToTable("Breeds");

        builder.HasKey(b => b.Id);

        builder.Property(b => b.Id)
            .HasConversion(
                id => id.Value,
                value => BreedId.Create(value));
        
        builder.Property(p => p.Name)    
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);

    }
}