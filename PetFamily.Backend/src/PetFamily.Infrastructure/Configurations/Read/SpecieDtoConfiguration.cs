using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class SpecieDtoConfiguration : IEntityTypeConfiguration<SpecieDto>
{
    public void Configure(EntityTypeBuilder<SpecieDto> builder)
    {
        builder.ToTable("species");

        builder.HasKey(v => v.Id);

        builder.Property(v => v.Name)
            .IsRequired()
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("name");

        builder.HasMany(v => v.Breeds)
            .WithOne(p => p.Specie)
            .HasForeignKey(p => p.SpecieId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}