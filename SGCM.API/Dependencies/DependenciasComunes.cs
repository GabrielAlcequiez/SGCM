using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Application.Services;
using SGCM.Application.Services.Seguridad_Usuarios;
using SGCM.Domain.Repository;
using SGCM.Infraestructure.Services;
using SGCM.Persistence.Repositories;

namespace SGCM.API.Dependencies;

public static class DependenciasComunes
{
    public static IServiceCollection AddDependenciasComunes(this IServiceCollection services)
    {
        services.AddHttpContextAccessor();
        services.AddScoped<ITokenService, TokenService>();
        services.AddScoped<IAutenticacionService, LoginService>();
        services.AddScoped<IUnitOfWork, UnitOfWork>();
        services.AddScoped<INotificacionService, NotificacionService>();

        return services;
    }
}
