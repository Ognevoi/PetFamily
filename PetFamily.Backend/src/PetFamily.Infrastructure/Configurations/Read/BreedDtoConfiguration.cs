using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using PetFamily.Application.Features.Volunteers.DTOs;
using PetFamily.Domain.Shared;

namespace PetFamily.Infrastructure.Configurations.Read;

public class BreedDtoConfiguration : IEntityTypeConfiguration<BreedDto>
{
    public void Configure(EntityTypeBuilder<BreedDto> builder)
    {
        builder.ToTable("breeds");

        builder.HasKey(b => b.Id);
        
        builder.Property(b => b.Name)
            .HasMaxLength(Constants.MAX_VERY_LOW_TEXT_LENGTH)
            .HasColumnName("name");
        
        builder.HasOne(b => b.Specie)
            .WithMany()
            .HasForeignKey(b => b.SpecieId)
            .IsRequired()
            .OnDelete(DeleteBehavior.Cascade);
    }
}