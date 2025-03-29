using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.Entities;
using PetFamily.Domain.PetManagement.Enums;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;
using PetFamily.Infrastructure.Extentions;

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

        builder.OwnsOne(p => p.Name, nb =>
        {
            nb.Property(n => n.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("name");
        });

        builder.OwnsOne(p => p.Description, db =>
        {
            db.Property(d => d.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                .HasColumnName("description");
        });

        builder.OwnsOne(p => p.Color, cb =>
        {
            cb.Property(c => c.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("color");
        });

        builder.OwnsOne(p => p.HealthInfo, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH)
                .HasColumnName("health_info");
        });

        builder.OwnsOne(p => p.Weight, wb =>
        {
            wb.Property(w => w.Value)
                .IsRequired()
                .HasColumnName("weight");
        });

        builder.OwnsOne(p => p.Height, hb =>
        {
            hb.Property(h => h.Value)
                .IsRequired()
                .HasColumnName("height");
        });

        builder.OwnsOne(p => p.IsSterilized, sb =>
        {
            sb.Property(s => s.Value)
                .IsRequired()
                .HasColumnName("is_sterilized");
        });

        builder.OwnsOne(p => p.IsVaccinated, vb =>
        {
            vb.Property(v => v.Value)
                .IsRequired()
                .HasColumnName("is_vaccinated");
        });

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
                .IsRequired(false)
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });

        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.Property(p => p.Photos)
            .JsonValueObjectCollectionConversion()
            .IsRequired()
            .HasColumnName("photos");
    }
}