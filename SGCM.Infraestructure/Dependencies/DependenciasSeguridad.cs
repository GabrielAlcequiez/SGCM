using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Application.Logger;
using SGCM.Application.Services.Seguridad_Usuarios;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Repository;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;
using SGCM.Persistence.Repositories;

namespace SGCM.Infraestructure.Dependencies;

public static class DependenciasSeguridad
{
    public static IServiceCollection AddDependenciasSeguridad(this IServiceCollection services)
    {
        services.AddScoped<IUsuarioAppService, UsuarioAppService>();
        services.AddScoped<IUsuarioDomainService, UsuarioDomainService>();
        services.AddScoped<IUsuarioRepository, UsuarioRepositoryEF>();
        
        services.AddScoped<IAdministradorAppService, AdministradorAppService>();
        services.AddScoped<IAdministradoresDomainService, AdministradoresDomainService>();
        services.AddScoped<IAdministradoresRepository, AdministradoresRepositoryEF>();
        
        services.AddScoped<IAuditoriaLogAppService, AuditoriaLogAppService>();
        services.AddScoped<IAuditoriaLogger, AuditoriaLogger>();
        services.AddScoped<IAuditoriaLogsRepository, AuditoriaLogRepositoryEF>();

        return services;
    }
}
