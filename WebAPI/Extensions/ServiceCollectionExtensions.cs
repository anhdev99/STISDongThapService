using System.Reflection;
using Core.Authorization;
using Core.Events;
using Core.Interfaces;
using Core.Interfaces.Settings;
using Core.Middleware;
using HealthChecks.UI.Client;
using Infrastructure.Data;
using Infrastructure.Data.SeedData;
using Infrastructure.Messaging.Consumers;
using Infrastructure.Repositories;
using Infrastructure.Services;
using Infrastructure.Settings;
using MediatR;
using Microsoft.AspNetCore.Diagnostics.HealthChecks;
using Microsoft.EntityFrameworkCore;
using Shared;
using Shared.Extensions;
using Shared.Interfaces;

namespace WebAPI.Extensions;


public static class ServiceCollectionExtensions
{
    public static void AddApplicationLayer(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddHealthChecks();
        services
            .AddTransient(typeof(IUnitOfWork), typeof(UnitOfWork))
            .AddTransient(typeof(IGenericRepository<>), typeof(GenericRepository<>));
        services
            .AddTransient<IMediator, Mediator>()
            .AddTransient<IDomainEventDispatcher, DomainEventDispatcher>();
        var connectionString = configuration.GetConnectionString("DefaultConnection");

        services.AddDbContext<ApplicationDbContext>(options =>
            options.UseNpgsql(connectionString,
                builder => builder.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName)));

        services.AddMediatR(cfg => cfg.RegisterServicesFromAssembly(Assembly.GetExecutingAssembly()));
        
        // Configs
        // services.AddConfigSection<IAppSettings, AppSettings>(configuration, nameof(AppSettings));
        services.AddConfigSection<IJwtSettings, JwtSettings>(configuration, nameof(JwtSettings));
  
        // Register Services
        services.AddScoped<IJwtUtils, JwtUtils>();
        services.AddScoped<IRankService, RankService>();
        services.AddScoped<ISectorService, SectorService>();
        services.AddScoped<IDepartmentService, DepartmentService>();
        services.AddScoped<IMenuService, MenuService>();
        services.AddScoped<IPermissionService, PermissionService>();
        services.AddScoped<IRoleService, RoleService>();
        services.AddScoped<IUserService, UserService>();
        services.AddScoped<IStatusService, ProjectStatusService>();
        services.AddScoped<ITaskTypeService, TaskTypeService>();
        services.AddScoped<IProjectService, ProjectService>();
        services.AddScoped<IIdentityService, IdentityService>();
        // Handlers
        services.AddTransient<INotificationHandler<FileDeletedEvent>, FileDeletedEventHandler>();

        services.AddSeeders();
    }

    public static async Task ConfigsApp(this WebApplication app)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Application is starting...");
        logger.LogInformation("Environment: {Environment}", app.Environment.EnvironmentName);
        logger.LogInformation("Application URL: http://localhost:5048");
        logger.LogInformation("Application URL: https://localhost:7077");

        Console.WriteLine("========================================");
        Console.WriteLine("         STIS Dong Thap Service is Running      ");
        Console.WriteLine("========================================");
        Console.WriteLine($"Environment: {app.Environment.EnvironmentName}");
        Console.WriteLine($"Swagger URL: http://localhost:5048/swagger");
        Console.WriteLine($"Swagger URL: https://localhost:7077/swagger");
        Console.WriteLine("========================================");
        app.UseAuthentication();
        app.UseAuthorization();

        app.MapHealthChecks(
            "/health",
            new HealthCheckOptions
            {
                ResponseWriter = UIResponseWriter.WriteHealthCheckUIResponse
            });
        app.MapControllers();
        app.UseMiddleware<ExceptionHandlingMiddleware>();
        app.UseMiddleware<JwtMiddleware>();
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }
        
        using (var scope = app.Services.CreateScope())
        {
            var seeders = scope.ServiceProvider.GetServices<ISeeder>();
            foreach (var seeder in seeders)
            {
                seeder.Seed();
            }
        }
    }

  
    private static void AddSeeders(this IServiceCollection services)
    {
        services.AddTransient<ISeeder, RoleSeeder>();
        services.AddTransient<ISeeder, DepartmentSeeder>();
        services.AddTransient<ISeeder, MenuSeeder>();
    } 
}