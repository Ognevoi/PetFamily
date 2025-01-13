using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.AnimalSpecies.Entities;
using PetFamily.Domain.AnimalSpecies.Value_Objects;
using PetFamily.Domain.Shared;


namespace PetFamily.Infrastructure.Configurations;

public class SpeciesConfiguration : IEntityTypeConfiguration<Species>
{
    public void Configure(EntityTypeBuilder<Species> builder)
    {
        builder.ToTable("species");

        builder.HasKey(s => s.Id);

        builder.Property(s => s.Id)
            .HasConversion(
                id => id.Value,
                value => SpeciesId.Create(value));

        builder.Property(s => s.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        
        builder.HasMany(s => s.Breed)
            .WithOne()
            .HasForeignKey("species_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);
    }
}