using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Pacientes;
using SGCM.Application.Services.Pacientes;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces.IPacientes;
using SGCM.Persistence.Repositories;

namespace SGCM.API.Dependencies;

public static class DependenciasPacientes
{
    public static IServiceCollection AddDependenciasPacientes(this IServiceCollection services)
    {
        services.AddScoped<IPacienteRepository, PacienteRepositoryEF>();
        services.AddScoped<IProveedoresRepository, ProveedoresRepositoryEF>();
        
        services.AddScoped<IPacienteAppService, PacienteAppService>();
        services.AddScoped<IPacienteDomainService, PacienteDomainService>();
        
        services.AddScoped<IProveedoresAppService, ProveedoresAppService>();
        services.AddScoped<IProveedoresDomainService, ProveedoresDomainService>();

        return services;
    }
}
