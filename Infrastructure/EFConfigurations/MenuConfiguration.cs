using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Infrastructure.EFConfigurations;

public class MenuConfiguration : IEntityTypeConfiguration<Menu>
{
    public void Configure(EntityTypeBuilder<Menu> builder)
    {
        builder.Property(p => p.Name)
            .IsRequired()
            .HasMaxLength(100);

        builder.Property(p => p.Url)
            .HasMaxLength(200);

        builder.Property(p => p.Description)
            .HasMaxLength(500);

        builder.Property(p => p.Icon)
            .HasMaxLength(100);

        builder.Property(p => p.IsBlank)
            .IsRequired();

        builder.Property(p => p.Order)
            .IsRequired();

        builder.HasOne(p => p.Parent)
            .WithMany(p => p.Children)
            .HasForeignKey(p => p.ParentId)
            .OnDelete(DeleteBehavior.Restrict);

        builder.HasMany(p => p.RoleMenus)
            .WithOne(p => p.Menu)
            .HasForeignKey(p => p.MenuId)
            .OnDelete(DeleteBehavior.Cascade);
    }
}