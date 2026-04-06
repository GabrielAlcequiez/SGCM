using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.ViewModels.Pacientes.Proveedor;
using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Controllers
{
    public class ProveedoresController : Controller
    {
        private readonly IProveedoresApiService _proveedoresApiService;

        public ProveedoresController(IProveedoresApiService proveedoresApiService)
        {
            _proveedoresApiService = proveedoresApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            try
            {
                var proveedores = await _proveedoresApiService.GetAllAsync(token);
                return View(proveedores);
            }
            catch (Exception)
            {
                return View(new List<ProveedoresResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var proveedor = await _proveedoresApiService.GetByIdAsync(token, id);
            if (proveedor == null) return NotFound();

            var viewModel = new ProveedorViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View(new CrearProveedorViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearProveedorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearProveedoresDto
                {
                    Nombre = model.Nombre,
                    RNC = model.RNC,
                    Telefono = model.Telefono,
                    CoberturaDefault = model.CoberturaDefault
                };

                var resultado = await _proveedoresApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear el proveedor.");
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

            var proveedor = await _proveedoresApiService.GetByIdAsync(token, id);
            if (proveedor == null) return NotFound();

            var viewModel = new EditarProveedorViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarProveedorViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearProveedoresDto
                {
                    Nombre = model.Nombre,
                    RNC = model.RNC,
                    Telefono = model.Telefono,
                    CoberturaDefault = model.CoberturaDefault
                };

                var resultado = await _proveedoresApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar el proveedor.");
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

            var proveedor = await _proveedoresApiService.GetByIdAsync(token, id);
            if (proveedor == null) return NotFound();

            var viewModel = new ProveedorViewModel
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _proveedoresApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
