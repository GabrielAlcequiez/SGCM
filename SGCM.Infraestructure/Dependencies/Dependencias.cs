using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using SGCM.Persistence.Context;

namespace SGCM.Infraestructure.Dependencies;

public static class Dependencias
{
    public static IServiceCollection AddDependencias(this IServiceCollection services, string connectionString)
    {
        services.AddDbContext<SGCMContext>(options => options.UseSqlServer(connectionString));
        
        services.AddDependenciasComunes();
        services.AddDependenciasSeguridad();
        services.AddDependenciasPacientes();
        services.AddDependenciasCitas();

        return services;
    }
}
