using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.ViewModels.Pacientes.Paciente;
using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Web.Controllers
{
    public class PacientesController : Controller
    {
        private readonly IPacienteApiService _pacienteApiService;
        private readonly IProveedoresApiService _proveedoresApiService;

        public PacientesController(IPacienteApiService pacienteApiService, IProveedoresApiService proveedoresApiService)
        {
            _pacienteApiService = pacienteApiService;
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
                var pacientes = await _pacienteApiService.GetAllAsync(token);
                return View(pacientes);
            }
            catch (Exception)
            {
                return View(new List<PacienteResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var paciente = await _pacienteApiService.GetByIdAsync(token, id);
            if (paciente == null) return NotFound();

            var viewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var model = new CrearPacienteViewModel();
            try
            {
                var proveedores = await _proveedoresApiService.GetAllAsync(token);
                model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList();
            }
            catch
            {
                model.ProveedoresDisponibles = new List<ProveedorDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearPacienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearPacienteDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    FechaNacimiento = model.FechaNacimiento,
                    ProveedorId = model.ProveedorId,
                    NSS = model.NSS
                };

                var resultado = await _pacienteApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear el paciente.");
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

            var paciente = await _pacienteApiService.GetByIdAsync(token, id);
            if (paciente == null) return NotFound();

            var model = new EditarPacienteViewModel
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };

            try
            {
                var proveedores = await _proveedoresApiService.GetAllAsync(token);
                model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList();
            }
            catch
            {
                model.ProveedoresDisponibles = new List<ProveedorDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarPacienteViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearPacienteDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    FechaNacimiento = model.FechaNacimiento,
                    ProveedorId = model.ProveedorId,
                    NSS = model.NSS
                };

                var resultado = await _pacienteApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar el paciente.");
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

            var paciente = await _pacienteApiService.GetByIdAsync(token, id);
            if (paciente == null) return NotFound();

            var viewModel = new PacienteViewModel
            {
                Id = paciente.Id,
                Nombre = paciente.Nombre,
                Apellido = paciente.Apellido,
                Telefono = paciente.Telefono,
                Direccion = paciente.Direccion,
                FechaNacimiento = paciente.FechaNacimiento,
                ProveedorId = paciente.ProveedorId,
                NSS = paciente.NSS
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _pacienteApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult CompletarPerfil()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            if (HttpContext.Session.GetInt32("PacienteId") != null)
            {
                return RedirectToAction("Index", "Citas");
            }

            var model = new CompletarPerfilViewModel();
            
            try
            {
                var proveedores = _proveedoresApiService.GetAllAsync(token).Result;
                model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList();
            }
            catch
            {
                model.ProveedoresDisponibles = new List<ProveedorDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarPerfil(CompletarPerfilViewModel model)
        {
            if (!ModelState.IsValid)
            {
                try
                {
                    var token = GetToken();
                    var proveedores = await _proveedoresApiService.GetAllAsync(token);
                    model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre
                    }).ToList();
                }
                catch { }
                return View(model);
            }

            try
            {
                var token = GetToken();
                var dto = new CrearPacienteDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    Direccion = model.Direccion,
                    FechaNacimiento = model.FechaNacimiento,
                    ProveedorId = model.ProveedorId,
                    NSS = model.NSS
                };

                var resultado = await _pacienteApiService.CreateAsync(token, dto);

                if (resultado != null)
                {
                    HttpContext.Session.SetInt32("PacienteId", resultado.Id);
                    return RedirectToAction("Index", "Citas");
                }
                ModelState.AddModelError(string.Empty, "No fue posible completar el perfil.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API.");
            }

            try
            {
                var token = GetToken();
                var proveedores = await _proveedoresApiService.GetAllAsync(token);
                model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                {
                    Id = p.Id,
                    Nombre = p.Nombre
                }).ToList();
            }
            catch { }

            return View(model);
        }
    }
}
