using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFConfigurations;

public class StatusConfiguration : IEntityTypeConfiguration<ProjectStatus>
{
    public void Configure(EntityTypeBuilder<ProjectStatus> builder)
    {
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);
        
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.Order)
            .IsRequired();
    }
}