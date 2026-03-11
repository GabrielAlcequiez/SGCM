using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;

namespace SGCM.Application.Services.Seguridad_Usuarios
{
    public class AdministradorAppService : IAdministradorAppService
    {
        private readonly IAdministradoresRepository _repository;
        private readonly IAdministradoresDomainService _domainService;
        private readonly IAuditoriaLogger _auditoriaLogger;
        public AdministradorAppService(IAdministradoresRepository repository, IAdministradoresDomainService domainService, IAuditoriaLogger auditoriaLogger)
        {
            _repository = repository;
            _domainService = domainService;
            _auditoriaLogger = auditoriaLogger;
        }

        public async Task<AdministradorResponseDto> CrearAsync(CrearAdministradorDto dto)
        {
            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación y autorización
            var nuevoAdministrador = new Administradores
            (
                usuarioIdTemp,
                dto.Nombre,
                dto.Apellido,
                dto.Cargo ?? string.Empty
            );

            await _repository.AgregarAsync(nuevoAdministrador);
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Crear", "Administrador", $"Creación del administrador con ID {nuevoAdministrador.Id}");

            return new AdministradorResponseDto
            {
                Id = nuevoAdministrador.Id,
                Nombre = nuevoAdministrador.Nombre,
                Apellido = nuevoAdministrador.Apellido,
                Cargo = nuevoAdministrador.Cargo
            };

        }

        public async Task<AdministradorResponseDto> ActualizarAsync(int id, CrearAdministradorDto dtoC)
        {
            var administradorExistente = await _repository.ObtenerPorIdAsync(id);

            if(administradorExistente is null)
                throw new ExcepcionNoEncontrado("Administradores", id);

            administradorExistente.Actualizar(dtoC.Nombre, dtoC.Apellido, dtoC.Cargo ?? string.Empty);

            await _repository.ActualizarAsync(administradorExistente);

            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación y autorización
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Administrador", $"Actualización del administrador con ID {administradorExistente.Id}");
            return new AdministradorResponseDto
            {
                Id = administradorExistente.Id,
                Nombre = administradorExistente.Nombre,
                Apellido = administradorExistente.Apellido,
                Cargo = administradorExistente.Cargo
            };
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var administradorExistente = await _repository.ObtenerPorIdAsync(id);
            if( administradorExistente is null)
               throw new ExcepcionNoEncontrado("Administradores", id);

            administradorExistente.Eliminar();
            await _repository.ActualizarAsync(administradorExistente);

            int usuarioIdTemp = 1; // Temporal hasta implementar autenticación
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Administrador", $"Eliminación del administrador con ID {id}");

            return true;
        }

        public async Task<AdministradorResponseDto> LeerAsync(int id)
        {
            var administradorExistente = await _repository.ObtenerPorIdAsync(id);
            if (administradorExistente is null)
                throw new ExcepcionNoEncontrado("Administradores", id);

            return new AdministradorResponseDto
            {
                Id = administradorExistente.Id,
                Nombre = administradorExistente.Nombre,
                Apellido = administradorExistente.Apellido,
                Cargo = administradorExistente.Cargo
            };
        }

        public async Task<IReadOnlyList<AdministradorResponseDto>> LeerTodosAsync()
        {
            var administradores = await _repository.ObtenerTodosAsync();

            return administradores.Select(a => new AdministradorResponseDto
            {
                Id = a.Id,
                Nombre = a.Nombre,
                Apellido = a.Apellido,
                Cargo = a.Cargo
            }).ToList();


        }
    }
}
