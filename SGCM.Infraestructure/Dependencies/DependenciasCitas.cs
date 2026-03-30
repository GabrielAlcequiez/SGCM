using Microsoft.Extensions.DependencyInjection;
using SGCM.Application.Interfaces.Citas_Agenda;
using SGCM.Application.Services.Citas_Agenda;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces.ICitas;
using SGCM.Persistence.Repositories;

namespace SGCM.Infraestructure.Dependencies;

public static class DependenciasCitas
{
    public static IServiceCollection AddDependenciasCitas(this IServiceCollection services)
    {
        services.AddScoped<ICitaRepository, CitasRepositoryEF>();
        services.AddScoped<IEspecialidadesRepository, EspecialidadesRepositoryEF>();
        services.AddScoped<IDisponibilidadRepository, DisponibilidadRepositoryEF>();
        services.AddScoped<IMedicoRepository, MedicosRepositoryEF>();
        
        services.AddScoped<ICitasAppService, CitasAppService>();
        services.AddScoped<ICitasDomainService, CitasDomainService>();
        
        services.AddScoped<IEspecialidadesAppService, EspecialidadesAppService>();
        services.AddScoped<IEspecialidadDomainService, EspecialidadDomainService>();
        
        services.AddScoped<IDisponibilidadAppService, DisponibilidadAppService>();
        services.AddScoped<IDisponibilidadDomainService, DisponibilidadDomainService>();
        
        services.AddScoped<IMedicoAppService, MedicoAppService>();
        services.AddScoped<IMedicoDomainService, MedicoDomainService>();

        return services;
    }
}
