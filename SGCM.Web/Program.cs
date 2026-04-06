using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.IdentityModel.Tokens;
using System.Text;
using SGCM.Web.Services;
using SGCM.Web.Services.Auth;
using SGCM.Web.Services.Seguridad_Usuarios;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Infraestructure.Dependencies;

namespace SGCM.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            #region Configuración HttpClient
            builder.Services.AddHttpClient<IAuthApiService, AuthApiService>();
            builder.Services.AddHttpClient<IUsuarioApiService, UsuarioApiService>();
            builder.Services.AddHttpClient<IAdministradorApiService, AdministradorApiService>();
            builder.Services.AddHttpClient<IAuditoriaApiService, AuditoriaApiService>();
            builder.Services.AddHttpClient<IPacienteApiService, PacienteApiService>();
            builder.Services.AddHttpClient<IProveedoresApiService, ProveedoresApiService>();
            builder.Services.AddHttpClient<IMedicoApiService, MedicoApiService>();
            builder.Services.AddHttpClient<IEspecialidadesApiService, EspecialidadesApiService>();
            builder.Services.AddHttpClient<IDisponibilidadApiService, DisponibilidadApiService>();
            builder.Services.AddHttpClient<ICitasApiService, CitasApiService>();
            builder.Services.AddScoped<BaseApiService>(sp =>
            {
                var httpClient = sp.GetRequiredService<IHttpClientFactory>().CreateClient();
                var configuration = sp.GetRequiredService<IConfiguration>();
                return new BaseApiService(httpClient, configuration);
            });
            builder.Services.AddScoped<IAuthApiService, AuthApiService>();
            builder.Services.AddScoped<IUsuarioApiService, UsuarioApiService>();
            builder.Services.AddScoped<IAdministradorApiService, AdministradorApiService>();
            builder.Services.AddScoped<IAuditoriaApiService, AuditoriaApiService>();
            builder.Services.AddScoped<IPacienteApiService, PacienteApiService>();
            builder.Services.AddScoped<IProveedoresApiService, ProveedoresApiService>();
            builder.Services.AddScoped<IMedicoApiService, MedicoApiService>();
            builder.Services.AddScoped<IEspecialidadesApiService, EspecialidadesApiService>();
            builder.Services.AddScoped<IDisponibilidadApiService, DisponibilidadApiService>();
            builder.Services.AddScoped<ICitasApiService, CitasApiService>();
            #endregion

            #region Configuración JWT para SGCM.Web
            var jwtKey = builder.Configuration["Jwt:Key"] ?? throw new InvalidOperationException("No se configuró Jwt:Key.");
            var jwtIssuer = builder.Configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("No se configuró Jwt:Issuer.");
            var jwtAudience = builder.Configuration["Jwt:Audience"] ?? throw new InvalidOperationException("No se configuró Jwt:Audience.");

            builder.Services
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
                .AddJwtBearer(options =>
                {
                    options.SaveToken = true;
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = jwtIssuer,
                        ValidAudience = jwtAudience,
                        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtKey))
                    };
                });

            builder.Services.AddAuthorization();
            #endregion

            #region Session para almacenar token
            builder.Services.AddDistributedMemoryCache();
            builder.Services.AddSession(options =>
            {
                options.IdleTimeout = TimeSpan.FromMinutes(30);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });
            #endregion

            builder.Services.AddDependencias(builder.Configuration.GetConnectionString("Database")!);

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
            }
            app.UseStaticFiles();

            app.UseRouting();

            app.UseSession();
            app.UseAuthentication();
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}
