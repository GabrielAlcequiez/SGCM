using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces;
using SGCM.Application.Services;
using SGCM.Domain.Repository;
using SGCM.Infraestructure.Notifications;
using SGCM.Infraestructure.Services;
using SGCM.Persistence.Repositories;
using IHttpContextAccessor = Microsoft.AspNetCore.Http.IHttpContextAccessor;
using HttpContextAccessor = Microsoft.AspNetCore.Http.HttpContextAccessor;

namespace SGCM.Infraestructure.Dependencies;

public static class DependenciasComunes
{
    public static IServiceCollection AddDependenciasComunes(this IServiceCollection services)
    {
        services.AddSingleton<IHttpContextAccessor, HttpContextAccessor>();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAutenticacionService, LoginService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<ISmsNotificationService, SmsNotificationService>();
        services.AddScoped<IPushNotificationService, PushNotificationService>();
        services.AddScoped<IEmailNotificationService, EmailNotificationService>();

        return services;
    }
}
