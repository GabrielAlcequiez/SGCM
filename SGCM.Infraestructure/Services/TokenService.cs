using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace SGCM.Infraestructure.Services
{
    public class TokenService : ITokenService
    {
        private readonly IConfiguration _configuration;
        private readonly IHttpContextAccessor _httpContextAccessor;

        public TokenService(IConfiguration configuration, IHttpContextAccessor httpContextAccessor)
        {
            _configuration = configuration;
            _httpContextAccessor = httpContextAccessor;
        }

        public string GenerarToken(int usuarioId, string email, string rol)
        {
            var key = _configuration["Jwt:Key"] ?? throw new InvalidOperationException("No se configuró Jwt:Key.");
            var issuer = _configuration["Jwt:Issuer"] ?? throw new InvalidOperationException("No se configuró Jwt:Issuer.");
            var audience = _configuration["Jwt:Audience"] ?? throw new InvalidOperationException("No se configuró Jwt:Audience.");
            var expiresMinutes = int.TryParse(_configuration["Jwt:ExpiresMinutes"], out var minutos) ? minutos : 60;

            var claims = new List<Claim>
            {
                new(JwtRegisteredClaimNames.Sub, usuarioId.ToString()),
                new(ClaimTypes.NameIdentifier, usuarioId.ToString()),
                new(ClaimTypes.Email, email),
                new(ClaimTypes.Role, rol),
                new(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
            };

            var signingCredentials = new SigningCredentials(
                new SymmetricSecurityKey(Encoding.UTF8.GetBytes(key)),
                SecurityAlgorithms.HmacSha256);

            var tokenDescriptor = new JwtSecurityToken(
                issuer: issuer,
                audience: audience,
                claims: claims,
                expires: DateTime.UtcNow.AddMinutes(expiresMinutes),
                signingCredentials: signingCredentials);

            return new JwtSecurityTokenHandler().WriteToken(tokenDescriptor);
        }

        // Permite que en la creación del usuario no haga kabum, ya que no hay un usuario autenticado aún, pero en otros casos sí se requiere un usuario autenticado.
        // En desarrollo (fase actual) retorna si no hay token, asi se evita el kabum, pero en producción sí lanza la excepción.
        public int ObtenerUsuarioIdActual()
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId) || usuarioId <= 0)
            {
                var entorno = _configuration["ASPNETCORE_ENVIRONMENT"];
                if (entorno == "Development")
                {
                    return 0;
                }
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado actual.");
            }

            return usuarioId;
        }

        public int ObtenerUsuarioIdActual(bool permitirAnonimo)
        {
            var usuarioIdClaim = _httpContextAccessor.HttpContext?.User?.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            if (!int.TryParse(usuarioIdClaim, out var usuarioId) || usuarioId <= 0)
            {
                if (permitirAnonimo)
                {
                    var entorno = _configuration["ASPNETCORE_ENVIRONMENT"];
                    if (entorno == "Development")
                    {
                        return 0;
                    }
                    return 0;
                }
                
                throw new UnauthorizedAccessException("No se pudo identificar al usuario autenticado actual.");
            }

            return usuarioId;
        }
    }
}
