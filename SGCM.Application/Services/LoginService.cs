using SGCM.Application.Dtos.Login;
using SGCM.Application.Interfaces;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;

namespace SGCM.Application.Services
{
    public class LoginService : IAutenticacionService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ITokenService _tokenService;

        public LoginService(IUsuarioRepository usuarioRepository, ITokenService tokenService)
        {
            _usuarioRepository = usuarioRepository;
            _tokenService = tokenService;
        }

        public Task<LoginResponseDto> LoginAsync(LoginDto request)
        {
            return LoginInternoAsync(request);
        }

        private async Task<LoginResponseDto> LoginInternoAsync(LoginDto request)
        {
            var usuario = await _usuarioRepository.ObtenerPorEmailAsync(request.Email);

            if (usuario is null || !BCrypt.Net.BCrypt.Verify(request.Password, usuario.PasswordHash))
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
