using SGCM.Web.Services;
using SGCM.Web.Services.Auth;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.Services.Seguridad_Usuarios;

namespace SGCM.Web.Extensiones
{
    public static class ServieCollecionExtensiones
    {

        public static IServiceCollection AddHttpClientFactory(this IServiceCollection services)
        {
            services.AddHttpClient<IAuthApiService, AuthApiService>();
            services.AddHttpClient<IUsuarioApiService, UsuarioApiService>();
            services.AddHttpClient<IAdministradorApiService, AdministradorApiService>();
            services.AddHttpClient<IAuditoriaApiService, AuditoriaApiService>();
            services.AddHttpClient<IPacienteApiService, PacienteApiService>();
            services.AddHttpClient<IProveedoresApiService, ProveedoresApiService>();
            services.AddHttpClient<IMedicoApiService, MedicoApiService>();
            services.AddHttpClient<IEspecialidadesApiService, EspecialidadesApiService>();
            services.AddHttpClient<IDisponibilidadApiService, DisponibilidadApiService>();
            services.AddHttpClient<ICitasApiService, CitasApiService>();

            services.AddScoped<BaseApiService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new BaseApiService(httpClient, configuration);
            });

            return services;
        }

        public static IServiceCollection AddApiServices(this IServiceCollection services)
        {
            services.AddScoped<IAuthApiService, AuthApiService>();
            services.AddScoped<IUsuarioApiService, UsuarioApiService>();
            services.AddScoped<IAdministradorApiService, AdministradorApiService>();
            services.AddScoped<IAuditoriaApiService, AuditoriaApiService>();
            services.AddScoped<IPacienteApiService, PacienteApiService>();
            services.AddScoped<IProveedoresApiService, ProveedoresApiService>();
            services.AddScoped<IMedicoApiService, MedicoApiService>();
            services.AddScoped<IEspecialidadesApiService, EspecialidadesApiService>();
            services.AddScoped<IDisponibilidadApiService, DisponibilidadApiService>();
            services.AddScoped<ICitasApiService, CitasApiService>();

            return services;
        }
    }

}

