using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Domain.PetManagement.AggregateRoot;
using PetFamily.Domain.PetManagement.ValueObjects;
using PetFamily.Domain.Shared;

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

        builder.ComplexProperty(v => v.FullName, fn =>
        {
            fn.Property(f => f.FirstName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("first_name");
            fn.Property(f => f.LastName)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("last_name");
        });
        
        builder.ComplexProperty(
            v => v.Email,
            pb =>
            {
                pb.Property(p => p.Value)
                    .IsRequired()
                    .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                    .HasColumnName("email");
            });
        
        builder.ComplexProperty(v => v.Description, d =>
        {
            d.Property(de => de.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
                .HasColumnName("description");
        });
        
        builder.ComplexProperty(v => v.ExperienceYears, ey =>
        {
            ey.Property(e => e.Value)
                .IsRequired()
                .HasColumnName("experience_years");
        });
        
        builder.ComplexProperty(v => v.PhoneNumber, pn =>
        {
            pn.Property(p => p.Value)
                .IsRequired()
                .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
                .HasColumnName("phone_number");
        });
        
        builder.OwnsOne(v => v.SocialNetworksList, sb =>
        {
            sb.ToJson("social_networks");

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
            ab.ToJson("assistance_details");

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
        
        builder.Property(p => p.CreatedAt)
            .HasDefaultValueSql("CURRENT_TIMESTAMP")
            .IsRequired();
        
        builder.Property(v => v.IsDeleted)
            .IsRequired()
            .HasColumnName("is_deleted");

        builder.Property(v => v.DeletedAt)
            .IsRequired()
            .HasColumnName("deleted_at");
    }
}
