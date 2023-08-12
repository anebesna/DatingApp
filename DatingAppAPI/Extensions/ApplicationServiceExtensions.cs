using DatingAppAPI.Data;
using DatingAppAPI.Helpers;
using DatingAppAPI.Interfaces;
using DatingAppAPI.Services;
using DatingAppAPI.SignalR;
using Microsoft.EntityFrameworkCore;

namespace DatingAppAPI.Extensions;

public static class ApplicationServiceExtensions
{
    public static IServiceCollection AddApplicationServices(this IServiceCollection services, IConfiguration config)
    {
        services.AddDbContext<DataContext>(opt =>
        {
            opt.UseSqlite(config.GetConnectionString("DefaultConnection"));
        });
        services.AddCors();
        services.AddScoped<ITokenService, TokenService>();
        services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        services.Configure<CloudinarySettings>(config.GetSection("CloudinarySettings"));
        services.AddScoped<IPhotoService, PhotoService>();
        services.AddScoped<LogUserActivity>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddSignalR(options => 
        { 
            options.EnableDetailedErrors = true; 
        });
        services.AddSingleton<PresenceTracker>();
        
        return services;
    }
}