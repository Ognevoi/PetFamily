using System.Text.Json;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class PetDtoConfiguration : IEntityTypeConfiguration<PetDto>
{
    public void Configure(EntityTypeBuilder<PetDto> builder)
    {
        builder.ToTable("pets");

        builder.HasKey(p => p.Id);

        builder.HasOne(p => p.Volunteer)
            .WithMany()
            .HasForeignKey(p => p.VolunteerId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(p => p.Name)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("name");

        builder.Property(p => p.Description)
            .HasMaxLength(Constants.MAX_MEDIUM_TEXT_LENGTH)
            .HasColumnName("description");

        builder.Property(p => p.Position)
            .HasColumnName("serial_number")
            .IsRequired();

        builder.Property(p => p.Photos)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasConversion(
                v => JsonSerializer.Serialize(v.Select(f => f.FileName), new JsonSerializerOptions()),
                v => JsonSerializer.Deserialize<IReadOnlyList<PhotoDto>>(v, new JsonSerializerOptions()) ??
                     new List<PhotoDto>()
            )
            .HasColumnName("photos");
    }
}