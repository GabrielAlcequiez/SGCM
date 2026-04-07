using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Auth;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.Services.Seguridad_Usuarios;
using SGCM.Web.ViewModels.Auth;

namespace SGCM.Web.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;
        private readonly IPacienteApiService _pacienteApiService;
        private readonly IMedicoApiService _medicoApiService;
        private readonly IAdministradorApiService _administradorApiService;

        public AuthController(
            IAuthApiService authApiService,
            IPacienteApiService pacienteApiService,
            IMedicoApiService medicoApiService,
            IAdministradorApiService administradorApiService)
        {
            _authApiService = authApiService;
            _pacienteApiService = pacienteApiService;
            _medicoApiService = medicoApiService;
            _administradorApiService = administradorApiService;
        }

        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            ViewBag.ReturnUrl = returnUrl;
            return View(new LoginViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                var loginDto = new Application.Dtos.Login.LoginDto
                {
                    Email = model.Email,
                    Password = model.Password
                };

                var result = await _authApiService.LoginAsync(loginDto);

                if (result == null || string.IsNullOrEmpty(result.Token))
                {
                    model.ErrorMessage = "Credenciales inválidas. Por favor verifique su correo y contraseña.";
                    return View(model);
                }

                HttpContext.Session.SetString("JWTToken", result.Token);
                HttpContext.Session.SetInt32("UsuarioId", result.UsuarioId);
                HttpContext.Session.SetString("UsuarioEmail", result.Email);
                HttpContext.Session.SetString("UsuarioRol", result.Rol);

                if (result.Rol == "Paciente")
                {
                    try
                    {
                        var paciente = await _pacienteApiService.GetByUsuarioIdAsync(result.Token, result.UsuarioId);
                        if (paciente != null)
                        {
                            HttpContext.Session.SetInt32("PacienteId", paciente.Id);
                        }
                    }
                    catch { }
                }

                if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                {
                    return Redirect(returnUrl);
                }

                return RedirectToAction("Index", "Home");
            }
            catch (HttpRequestException)
            {
                model.ErrorMessage = "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.";
                return View(model);
            }
            catch (Exception)
            {
                model.ErrorMessage = "Ocurrió un error inesperado. Por favor intente más tarde.";
                return View(model);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            HttpContext.Session.SetString("TempEmail", model.Email);
            HttpContext.Session.SetString("TempPassword", model.Password);
            HttpContext.Session.SetString("TempRol", model.Rol);

            return model.Rol switch
            {
                "Medico" => RedirectToAction("CompletarPerfil", "Medicos"),
                "Administrador" => RedirectToAction("CompletarPerfil", "Administradores"),
                _ => RedirectToAction("CompletarPerfil", "Pacientes")
            };
        }

        [HttpGet]
        public IActionResult CancelarRegistro()
        {
            HttpContext.Session.Remove("TempEmail");
            HttpContext.Session.Remove("TempPassword");
            HttpContext.Session.Remove("TempRol");
            return RedirectToAction("Login", "Auth");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Login", "Auth");
        }

        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }
    }
}
