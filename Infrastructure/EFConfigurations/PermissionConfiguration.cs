using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Core.Entities;

namespace Infrastructure.EFConfigurations;

public class PermissionConfiguration : IEntityTypeConfiguration<Permission>
{
    public void Configure(EntityTypeBuilder<Permission> builder)
    {
        builder.Property(p => p.Code)
            .IsRequired()
            .HasMaxLength(100);
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Order)
            .IsRequired();
        builder.Property(p => p.Priority)
            .IsRequired();
        builder.Property(p => p.IsProtected)
            .IsRequired();

        builder.HasMany(p => p.RolePermissions)
            .WithOne(p => p.Permission)
            .HasForeignKey(p => p.PermissionId);
    }
}