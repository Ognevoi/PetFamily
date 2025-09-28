using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class VolunteerDtoConfiguration : IEntityTypeConfiguration<VolunteerDto>
{
    public void Configure(EntityTypeBuilder<VolunteerDto> builder)
    {
        builder.ToTable("volunteers");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.FirstName)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("first_name");

        builder.Property(v => v.LastName)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("last_name");

        builder.Property(v => v.Email)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("email");

        builder.Property(v => v.Description)
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .HasColumnName("description");

        builder.Property(v => v.ExperienceYears)
            .HasColumnName("experience_years");

        builder.Property(v => v.PhoneNumber)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("phone_number");

        builder.Property(v => v.CreatedAt)
            .HasColumnName("created_at");

        builder.Property(v => v.IsDeleted)
            .HasColumnName("is_deleted");

        builder.HasMany(v => v.Pets)
            .WithOne(p => p.Volunteer)
            .HasForeignKey(p => p.VolunteerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.HasQueryFilter(v => v.IsDeleted == false);
    }
}