using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Seguridad_Usuarios;
using SGCM.Web.Services.Auth;
using SGCM.Web.ViewModels.Seguridad_Usuario.Administrador;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Application.Dtos.Login;

namespace SGCM.Web.Controllers
{
    public class AdministradoresController : Controller
    {
        private readonly IAdministradorApiService _administradorApiService;
        private readonly IAuthApiService _authApiService;

        public AdministradoresController(
            IAdministradorApiService administradorApiService,
            IAuthApiService authApiService)
        {
            _administradorApiService = administradorApiService;
            _authApiService = authApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var administradores = await _administradorApiService.GetAllAsync(token);
            return View(administradores);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var administrador = await _administradorApiService.GetByIdAsync(token, id);
            if (administrador == null) return NotFound();

            var viewModel = new AdministradorViewModel
            {
                Id = administrador.Id,
                Nombre = administrador.Nombre,
                Apellido = administrador.Apellido,
                Cargo = administrador.Cargo
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View(new CrearAdministradorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearAdministradorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearAdministradorDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cargo = model.Cargo
                };

                var resultado = await _administradorApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear el administrador.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor intente más tarde.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var administrador = await _administradorApiService.GetByIdAsync(token, id);
            if (administrador == null) return NotFound();

            var viewModel = new AdministradorViewModel
            {
                Id = administrador.Id,
                Nombre = administrador.Nombre,
                Apellido = administrador.Apellido,
                Cargo = administrador.Cargo
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, AdministradorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearAdministradorDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cargo = model.Cargo
                };

                var resultado = await _administradorApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar el administrador.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor intente más tarde.");
            }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var administrador = await _administradorApiService.GetByIdAsync(token, id);
            if (administrador == null) return NotFound();

            var viewModel = new AdministradorViewModel
            {
                Id = administrador.Id,
                Nombre = administrador.Nombre,
                Apellido = administrador.Apellido,
                Cargo = administrador.Cargo
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _administradorApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CompletarPerfil()
        {
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("TempEmail") == null)
            {
                return RedirectToAction("Register", "Auth");
            }

            if (HttpContext.Session.GetInt32("AdministradorId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            return View(new CrearAdministradorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarPerfil(CrearAdministradorViewModel model)
        {
            var tempEmail = HttpContext.Session.GetString("TempEmail");
            var tempPassword = HttpContext.Session.GetString("TempPassword");
            var tempRol = HttpContext.Session.GetString("TempRol");

            if (string.IsNullOrEmpty(tempEmail) || string.IsNullOrEmpty(tempPassword) || string.IsNullOrEmpty(tempRol))
            {
                return RedirectToAction("Register", "Auth");
            }

            if (!ModelState.IsValid) return View(model);

            try
            {
                var registerDto = new RegisterDto
                {
                    Email = tempEmail,
                    Password = tempPassword,
                    Rol = tempRol
                };

                var result = await _authApiService.RegisterAsync(registerDto);

                if (result == null)
                {
                    ModelState.AddModelError(string.Empty, "No fue posible completar el registro. El correo podría estar en uso.");
                    return View(model);
                }

                HttpContext.Session.SetString("JWTToken", result.Token);
                HttpContext.Session.SetInt32("UsuarioId", result.UsuarioId);
                HttpContext.Session.SetString("UsuarioEmail", result.Email);
                HttpContext.Session.SetString("UsuarioRol", result.Rol);

                HttpContext.Session.Remove("TempEmail");
                HttpContext.Session.Remove("TempPassword");
                HttpContext.Session.Remove("TempRol");

                var token = result.Token;

                var dto = new CrearAdministradorDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Cargo = model.Cargo
                };

                var adminResult = await _administradorApiService.CreateAsync(token, dto);

                if (adminResult != null)
                {
                    HttpContext.Session.SetInt32("AdministradorId", adminResult.Id);
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError(string.Empty, "No fue posible completar el perfil.");
            }
            catch (HttpRequestException)
            {
                HttpContext.Session.Remove("TempEmail");
                HttpContext.Session.Remove("TempPassword");
                HttpContext.Session.Remove("TempRol");
                HttpContext.Session.Clear();
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor intente más tarde.");
            }

            return View(model);
        }
    }
}
