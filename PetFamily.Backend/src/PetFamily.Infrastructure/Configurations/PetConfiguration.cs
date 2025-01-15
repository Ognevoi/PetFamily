using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.AnimalSpecies.Entities;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.Enums;
using PetFamily.Domain.Volunteers.ValueObjects;

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
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(p => p.Breed)
            .WithMany() 
            .HasForeignKey("breed_id")
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Description)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.Color)
            .IsRequired(false)
            .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);

        builder.Property(p => p.HealthInfo)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);

        builder.Property(p => p.Weight)
            .IsRequired(false);

        builder.Property(p => p.Height)
            .IsRequired(false);

        builder.Property(p => p.IsSterilized);

        builder.Property(p => p.IsVaccinated);

        builder.Property(p => p.BirthDate)
            .IsRequired(false);

        builder.Property(p => p.PetStatus)
            .HasConversion(
                status => status.ToString(),
                value => Enum.Parse<PetStatus>(value))
            .IsRequired();
        
        builder.OwnsOne(p => p.AssistanceDetails, ab => 
        {
            ab.ToJson();

            ab.OwnsMany(a => a.AssistanceDetails, ad =>
                {
                    ad.Property(a => a.Name)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                        .HasColumnName("assistance_details_name");
                    ad.Property(a => a.Description)
                        .IsRequired()
                        .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                        .HasColumnName("assistance_details_description");
                }
            );
        });
        
        builder.ComplexProperty(p => p.Address, a =>
        {
            a.Property(ad => ad.Street)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                .HasColumnName("street");
            a.Property(ad => ad.City)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("city");
            a.Property(ad => ad.State)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("state");
            a.Property(ad => ad.ZipCode)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("zip_code");
        });
        
        builder.OwnsOne(p => p.PhoneNumber, pnb =>
        {
            pnb.Property(pn => pn.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.OwnsOne(p => p.PetPhoto, ppb =>
        {
            ppb.ToJson();
            
            ppb.Property(pp => pp.Url)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            
            ppb.Property(pp => pp.FileName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
        });
    }
}