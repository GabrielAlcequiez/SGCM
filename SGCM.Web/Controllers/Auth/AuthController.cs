using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Auth;
using SGCM.Web.ViewModels.Auth;

namespace SGCM.Web.Controllers.Auth
{
    public class AuthController : Controller
    {
        private readonly IAuthApiService _authApiService;

        public AuthController(IAuthApiService authApiService)
        {
            _authApiService = authApiService;
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

            try
            {
                var registerDto = new Application.Dtos.Login.RegisterDto
                {
                    Email = model.Email,
                    Password = model.Password,
                    Rol = model.Rol
                };

                var result = await _authApiService.RegisterAsync(registerDto);

                if (result == null)
                {
                    model.ErrorMessage = "No fue posible completar el registro. El correo podría estar en uso.";
                    return View(model);
                }

                HttpContext.Session.SetString("JWTToken", result.Token);
                HttpContext.Session.SetInt32("UsuarioId", result.UsuarioId);
                HttpContext.Session.SetString("UsuarioEmail", result.Email);
                HttpContext.Session.SetString("UsuarioRol", result.Rol);

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
