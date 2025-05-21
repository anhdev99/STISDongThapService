using System.Reflection;
using Core.Entities;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Interfaces;

namespace Infrastructure.Data;

public partial class ApplicationDbContext : DbContext
{
    private readonly IDomainEventDispatcher _dispatcher;

    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options,
        IDomainEventDispatcher dispatcher)
        : base(options)
    {
        _dispatcher = dispatcher;
        AppContext.SetSwitch("Npgsql.EnableLegacyTimestampBehavior", true);
        AppContext.SetSwitch("Npgsql.DisableDateTimeInfinityConversions", true);
    }

    // Authentication
    public DbSet<Role> Roles => Set<Role>();
    public DbSet<Permission> Permissions => Set<Permission>();
    public DbSet<Menu> Menus => Set<Menu>();
    public DbSet<RefreshToken> RefreshTokens => Set<RefreshToken>();
    public DbSet<UserRole> UserRoles => Set<UserRole>();
    public DbSet<RolePermission> RolePermissions => Set<RolePermission>();
    public DbSet<RoleMenu> RoleMenus => Set<RoleMenu>();
    public DbSet<ProjectStatus> Status => Set<ProjectStatus>();
    public DbSet<FileMetadata> FileMetadata => Set<FileMetadata>();
    public DbSet<Department> Departments => Set<Department>();
  
    // Business Login
    public DbSet<Project> Projects => Set<Project>();
    public DbSet<Notification> Notifications => Set<Notification>();
    public DbSet<UserNotification> UserNotifications => Set<UserNotification>();
    public DbSet<GoverningAgency> GoverningAgencies => Set<GoverningAgency>();
    public DbSet<HostOrganization> HostOrganizations => Set<HostOrganization>();
    public DbSet<ManagementLevel> ManagementLevels => Set<ManagementLevel>();
    public DbSet<ProjectLeader> ProjectLeaders => Set<ProjectLeader>();
    public DbSet<Rank> Ranks => Set<Rank>();
    public DbSet<Sector> Sectors => Set<Sector>();
    public DbSet<TaskType> TaskTypes => Set<TaskType>();
    
    public DbSet<User> User => Set<User>();
    public DbSet<UserSession> UserSessions => Set<UserSession>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        UpdateTimestamps();
        int result = await base.SaveChangesAsync(cancellationToken).ConfigureAwait(false);

        if (_dispatcher == null) return result;

        var entitiesWithEvents = ChangeTracker.Entries<BaseEntity>()
            .Select(e => e.Entity)
            .Where(e => e.DomainEvents.Any())
            .ToArray();

        await _dispatcher.DispatchAndClearEvents(entitiesWithEvents);

        return result;
    }

    public override int SaveChanges()
    {
        UpdateTimestamps();
        return SaveChangesAsync().GetAwaiter().GetResult();
    }

    private void UpdateTimestamps()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseAuditableEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseAuditableEntity)entry.Entity;

            if (entry.State == EntityState.Added)
            {
                entity.CreatedDate = DateTime.UtcNow;
            }

            entity.UpdatedDate = DateTime.UtcNow;
        }
    }
}