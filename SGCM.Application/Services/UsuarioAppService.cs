using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Services.Interfaces.ISeguridad_Usuarios;

namespace SGCM.Application.Services
{
    public class UsuarioAppService : IUsuarioAppService
    {
        private readonly IUsuarioRepository _repository;
        private readonly IAuditoriaLogger _auditoriaLogger;
        private readonly IUsuarioDomainService _usuarioDomainService;

        public UsuarioAppService(IUsuarioRepository repository, IAuditoriaLogger auditoriaLogger, IUsuarioDomainService usuarioDomain)
        {
            _repository = repository;
            _auditoriaLogger = auditoriaLogger;
            _usuarioDomainService = usuarioDomain;
        }


        public async Task<UsuarioResponseDto> CrearAsync(CrearUsuarioDto dto)
        {
            var passwordHash = BCrypt.Net.BCrypt.HashPassword(dto.password, workFactor: 12);

            var usuario = new Usuario
            (
                dto.email,
                passwordHash,
                dto.Rol
            );

            await _usuarioDomainService.ValidarEmailUnicoAsync(usuario.Email);

            await _repository.AgregarAsync(usuario);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Crear", "Usuario", $"Se creó un nuevo usuario con email: {dto.email}");

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };

        }
        public async Task<UsuarioResponseDto> ActualizarAsync(int id, CrearUsuarioDto dto)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);
            if (usuario is null)
                throw new ExcepcionNoEncontrado("Usuario", id);

            await _usuarioDomainService.ValidarEmailUnicoAsync(dto.email);

            usuario.Actualizar(dto.email, dto.Rol);

            await _repository.ActualizarAsync(usuario);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Usuario", $"Se actualizó el usuario con ID: {usuario.Id}");

            return new UsuarioResponseDto
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
        }

        public async Task CambiarPasswordAsync(int id, CambiarPasswordUsuarioDto dto)
        {
            var usuario = await _repository.ObtenerPorIdAsync(id);
            if (usuario is null)
                throw new ExcepcionNoEncontrado("Usuario", id);

            bool passwordValida = BCrypt.Net.BCrypt.Verify(dto.PasswordActual, usuario.PasswordHash);
            if(!passwordValida)
                throw new ExcepcionReglaNegocio("La contraseña actual es incorrecta", "PASSWORD_INCORRECTA");

            var nuevoPasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.NuevoPassword, workFactor: 12);
            usuario.ActualizarPassword(nuevoPasswordHash);

            await _repository.ActualizarAsync(usuario);

            int usuarioIdTemp = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "CambiarPassword", "Usuario", $"Se cambió el password del usuario con ID: {usuario.Id}");
        }

        public async Task<bool> EliminarAsync(int id)
        {
            var user = await _repository.ObtenerPorIdAsync(id);

            if (user is null)
                throw new ExcepcionNoEncontrado("Usuario", id);

            await _usuarioDomainService.PuedeEliminarUsuarioAsync(user.Id);

            await _repository.EliminarAsync(id);

            int usuarioTempId = 0;
            await _auditoriaLogger.RegistrarAsync(usuarioTempId, "Eliminar", "Usuario", $"El usuario con ID: {id} fue eliminado correctamente");
            return true;


        }

        public async Task<UsuarioResponseDto> LeerAsync(int id)
        {
            var user = await _repository.ObtenerPorIdAsync(id);
            if (user is null)
                throw new ExcepcionNoEncontrado("Usuario", id);
            return new UsuarioResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Rol = user.Rol,
                FechaRegistro = user.FechaRegistro
            };

        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> LeerTodosAsync()
        {
            var users = await _repository.ObtenerTodosAsync();

            if (!users.Any())
                throw new ExcepcionReglaNegocio("No hay usuarios registrados", "USUARIOS_NO_ENCONTRADOS");

            var listaUsers = users.Select(user => new UsuarioResponseDto
            {
                Id = user.Id,
                Email = user.Email,
                Rol = user.Rol,
                FechaRegistro = user.FechaRegistro
            }).ToList();

            return listaUsers;
        }

        public async Task<IReadOnlyList<UsuarioResponseDto>> LeerPorRolAsync(string rol)
        {
            var usuarios = await _repository.ObtenerPorRolAsync(rol);

            if (!usuarios.Any())
                throw new ExcepcionReglaNegocio($"No hay usuarios con el rol '{rol}'", "USUARIOS_ROL_NO_ENCONTRADOS");

            return usuarios.Select(u => new UsuarioResponseDto
            {
                Id = u.Id,
                Email = u.Email,
                Rol = u.Rol,
                FechaRegistro = u.FechaRegistro
            }).ToList();
        }


    }
}
