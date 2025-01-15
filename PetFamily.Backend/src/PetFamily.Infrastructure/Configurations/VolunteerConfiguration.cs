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
        
        builder.OwnsOne(v => v.SocialNetworksList, sb =>
        {
            sb.ToJson();

            sb.OwnsMany(s => s.SocialNetworks, sn =>
            {
                sn.Property(s => s.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
                sn.Property(s => s.Url)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
            });
        });
            
        builder.OwnsOne(v => v.AssistanceDetailsList, ab =>
        {
            ab.ToJson();

            ab.OwnsMany(a => a.AssistanceDetails, ad =>
            {
                ad.Property(a => a.Name)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH);
                ad.Property(a => a.Description)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH);
            }
                );
        });

        builder.HasMany(v => v.Pets)
            .WithOne()
            .HasForeignKey("volunteer_id")
            .IsRequired(false);
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
    }
}
