using PetFamily.Domain.Volunteers;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.Shared;
using PetFamily.Domain.Volunteers.Entities;
using PetFamily.Domain.Volunteers.ValueObjects;

namespace PetFamily.Infrastructure.Configurations;


public class VolunteerConfiguration : IEntityTypeConfiguration<Volunteer>
{
    public void Configure(EntityTypeBuilder<Volunteer> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Id)
            .HasConversion(
                id => id.Value,
                value => VolunteerId.Create(value));

        builder.Property(v => v.FullName)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        
        builder.OwnsOne(v => v.Email, e =>
        {
            e.Property(em => em.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        });
        
        builder.Property(v => v.Description)
            .IsRequired()
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
        
        builder.Property(v => v.ExperienceYears)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        
        builder.OwnsOne(v => v.PhoneNumber, pn =>
        {
            pn.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        });
        
        builder.OwnsMany(v => v.SocialNetworks, s =>
        {
            s.WithOwner().HasForeignKey("volunteer_id");
            s.Property(sn => sn.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
            s.Property(sn => sn.Url)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
        });
            
        builder.OwnsMany(v => v.AssistanceDetails, a =>
        {
            a.WithOwner()
                .HasForeignKey("volunteer_id");
            a.Property(ad => ad.Name)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LOW_TEXT_LENGTH);
            a.Property(ad => ad.Description)
                .IsRequired()
                .HasMaxLength(Constants.MAX_LONG_TEXT_LENGTH);
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id");
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();

    }
}
