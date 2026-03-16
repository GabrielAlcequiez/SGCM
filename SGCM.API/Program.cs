using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Pacientes;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Application.Logger;
using SGCM.Application.Services;
using SGCM.Application.Services.Pacientes;
using SGCM.Application.Services.Seguridad_Usuarios;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services;
using SGCM.Domain.Services.Interfaces.IPacientes;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;
using SGCM.Infraestructure.Services;
using SGCM.Persistence.Context;
using SGCM.Persistence.Repositories;
using System.Text;

namespace SGCM.API
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.

            #region Dependencias 
            builder.Services.AddDbContext<SGCMContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Database")));

            builder.Services.AddControllers();
            builder.Services.AddHttpContextAccessor();
            builder.Services.AddScoped<ITokenService, TokenService>();
            builder.Services.AddScoped<IAutenticacionService, LoginService>();

            builder.Services.AddScoped<IUsuarioAppService, UsuarioAppService>();
            builder.Services.AddScoped<IUsuarioDomainService, UsuarioDomainService>();
            builder.Services.AddScoped<IUsuarioRepository, UsuarioRepositoryEF>();
            builder.Services.AddScoped<IAdministradorAppService, AdministradorAppService>();
            builder.Services.AddScoped<IAdministradoresDomainService, AdministradoresDomainService>();
            builder.Services.AddScoped<IAdministradoresRepository, AdministradoresRepositoryEF>();
            builder.Services.AddScoped<IAuditoriaLogAppService, AuditoriaLogAppService>();
            builder.Services.AddScoped<IAuditoriaLogger, AuditoriaLogger>();
            builder.Services.AddScoped<IAuditoriaLogsRepository, AuditoriaLogRepositoryEF>();
            builder.Services.AddScoped<IPacienteRepository, PacienteRepositoryEF>();
            builder.Services.AddScoped<IMedicoRepository, MedicosRepositoryEF>();
            builder.Services.AddScoped<IProveedoresRepository, ProveedoresRepositoryEF>();
            builder.Services.AddScoped<IPacienteAppService, PacienteAppService>();
            builder.Services.AddScoped<IPacienteDomainService, PacienteDomainService>();
            builder.Services.AddScoped<IProveedoresAppService, ProveedoresAppService>();
            builder.Services.AddScoped<IProveedoresDomainService, ProveedoresDomainService>();
            builder.Services.AddScoped<ICitaRepository, CitasRepositoryEF>();
            builder.Services.AddScoped<IUnitOfWork, UnitOfWork>();


            #endregion

            #region Configuración JWT

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

            // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
            builder.Services.AddEndpointsApiExplorer();

            // necesario para poder autenticarme desde swagger con token

            builder.Services.AddSwaggerGen(c =>
            {
                c.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Description = "JWT Authorization header using the Bearer scheme. Enter 'Bearer' [space] and then your token.",
                    Name = "Authorization",
                    In = ParameterLocation.Header,
                    Type = SecuritySchemeType.ApiKey,
                    Scheme = "Bearer"
                });
                c.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer"
                            }
                        },
                        Array.Empty<string>()
                    }
                });
            });

            var app = builder.Build();

            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
            }

            app.UseAuthentication();
            app.UseAuthorization();


            app.MapControllers();

            app.Run();
        }
    }
}
