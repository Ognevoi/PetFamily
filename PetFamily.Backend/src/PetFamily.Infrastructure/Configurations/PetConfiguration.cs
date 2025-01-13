using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.AnimalSpecies;
using PetFamily.Domain.AnimalSpecies.Entities;
using PetFamily.Domain.Pets;
using PetFamily.Domain.Pets.Entities;
using PetFamily.Domain.Pets.Enums;
using PetFamily.Domain.Pets.ValueObjects;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations;

public class PetConfiguration : IEntityTypeConfiguration<Pet>
{
    public void Configure(EntityTypeBuilder<Pet> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.Property(p => p.Id)
            .HasConversion(
                id => id.Value,
                value => PetId.Create(value));

        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);

        builder.HasOne<Species>()
            .WithMany()
            .HasForeignKey("species_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasOne(p => p.Breed)
            .WithMany() 
            .HasForeignKey("breed_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Restrict);

        builder.Property(p => p.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.Color)
            .IsRequired()
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.HealthInfo)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.Property(p => p.Weight)
            .IsRequired();

        builder.Property(p => p.Height)
            .IsRequired();

        builder.Property(p => p.IsSterilized);

        builder.Property(p => p.IsVaccinated);

        builder.Property(p => p.BirthDate)
            .IsRequired();

        builder.Property(p => p.PetStatus)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<PetStatus>(value))
            .IsRequired();

        builder.OwnsMany(p => p.AssistanceDetails, a =>
        {
            a.WithOwner()
                .HasForeignKey("pet_id");
            a.Property(ad => ad.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            a.Property(ad => ad.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        });

        builder.OwnsOne(p => p.Address, a =>
        {
            a.Property(ad => ad.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            a.Property(ad => ad.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
            a.Property(ad => ad.State)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
            a.Property(ad => ad.ZipCode)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        });

        builder.OwnsOne(p => p.PhoneNumber, pn =>
        {
            pn.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        });

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.OwnsOne(p => p.PetPhoto, pp =>
        {
            pp.ToJson();
            
            pp.Property(pp => pp.Url)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            
            pp.Property(pp => pp.FileName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
            
    }
}