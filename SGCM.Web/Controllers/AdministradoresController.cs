using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.ViewModels.Seguridad_Usuario.Administrador;
using SGCM.Application.Dtos.Seguridad_Usuarios;
using SGCM.Web.Services.Seguridad_Usuarios;

namespace SGCM.Web.Controllers
{
    public class AdministradoresController : Controller
    {
        private readonly IAdministradorApiService _administradorApiService;

        public AdministradoresController(IAdministradorApiService administradorApiService)
        {
            _administradorApiService = administradorApiService;
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
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API.");
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
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API.");
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
    }
}
