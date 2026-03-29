using SGCM.Application.Dtos.Login;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Interfaces;
using SGCM.Application.Interfaces.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Infraestructure.Services;
using BCryptNet = BCrypt.Net.BCrypt;

namespace SGCM.Application.Services
{
    public class LoginService : IAutenticacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IUsuarioAppService _usuarioAppService;
        private readonly ITokenService _tokenService;

        public LoginService(IUsuarioRepository usuarioRepository, IUsuarioAppService usuarioAppService, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _usuarioAppService = usuarioAppService;
            _tokenService = tokenService;
        }

        public Task<LoginResponseDto> LoginAsync(LoginDto request)
        {
            return LoginInternoAsync(request);
        }

        public async Task<LoginResponseDto> RegisterAsync(RegisterDto request)
        {
            var existingUser = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);
            if (existingUser != null)
            {
                throw new ExcepcionReglaNegocio("El email ya está registrado.", "EMAIL_YA_EXISTE");
            }

            var crearDto = new CrearUsuarioDto
            {
                email = request.Email,
                password = request.Password,
                Rol = request.Rol
            };

            var usuario = await _usuarioAppService.CrearAsync(crearDto);
            var token = _tokenService.GenerarToken(usuario.Id, usuario.Email, usuario.Rol);

            return new LoginResponseDto
            {
                UsuarioId = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = token
            };
        }

        private async Task<LoginResponseDto> LoginInternoAsync(LoginDto request)
        {
            var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);

            if (usuario is null || !BCryptNet.Verify(request.Password, usuario.PasswordHash))
            {
                throw new ExcepcionReglaNegocio("Credenciales inválidas.", "CREDENCIALES_INVALIDAS");
            }

            var token = _tokenService.GenerarToken(usuario.Id, usuario.Email, usuario.Rol);

            return new LoginResponseDto
            {
                UsuarioId = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                Token = token
            };
        }
    }
}
