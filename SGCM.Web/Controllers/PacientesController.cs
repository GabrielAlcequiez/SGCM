using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Pacientes;
using SGCM.Web.Services.Auth;
using SGCM.Web.ViewModels.Pacientes.Paciente;
using SGCM.Application.Dtos.Pacientes;
using SGCM.Application.Dtos.Login;

namespace SGCM.Web.Controllers
{
    public class PacientesController : Controller
    {
        private readonly IPacienteApiService _pacienteApiService;
        private readonly IProveedoresApiService _proveedoresApiService;
        private readonly IAuthApiService _authApiService;

        public PacientesController(
            IPacienteApiService pacienteApiService,
            IProveedoresApiService proveedoresApiService,
            IAuthApiService authApiService)
        {
            _pacienteApiService = pacienteApiService;
            _proveedoresApiService = proveedoresApiService;
            _authApiService = authApiService;
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
            if (HttpContext.Session.GetString("JWTToken") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            if (HttpContext.Session.GetString("TempEmail") == null)
            {
                return RedirectToAction("Register", "Auth");
            }

            if (HttpContext.Session.GetInt32("PacienteId") != null)
            {
                return RedirectToAction("Index", "Citas");
            }

            var model = new CompletarPerfilViewModel();
            
            try
            {
                var token = GetToken();
                if (string.IsNullOrEmpty(token))
                {
                    model.ProveedoresDisponibles = new List<ProveedorDropdownViewModel>();
                    return View(model);
                }
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
            var tempEmail = HttpContext.Session.GetString("TempEmail");
            var tempPassword = HttpContext.Session.GetString("TempPassword");
            var tempRol = HttpContext.Session.GetString("TempRol");

            if (string.IsNullOrEmpty(tempEmail) || string.IsNullOrEmpty(tempPassword) || string.IsNullOrEmpty(tempRol))
            {
                return RedirectToAction("Register", "Auth");
            }

            if (!ModelState.IsValid)
            {
                try
                {
                    var token = GetToken();
                    if (!string.IsNullOrEmpty(token))
                    {
                        var proveedores = await _proveedoresApiService.GetAllAsync(token);
                        model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                        {
                            Id = p.Id,
                            Nombre = p.Nombre
                        }).ToList();
                    }
                }
                catch { }
                return View(model);
            }

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

                var pacienteResult = await _pacienteApiService.CreateAsync(token, dto);

                if (pacienteResult != null)
                {
                    HttpContext.Session.SetInt32("PacienteId", pacienteResult.Id);
                    return RedirectToAction("Index", "Citas");
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

            try
            {
                var token = GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    var proveedores = await _proveedoresApiService.GetAllAsync(token);
                    model.ProveedoresDisponibles = proveedores.Select(p => new ProveedorDropdownViewModel
                    {
                        Id = p.Id,
                        Nombre = p.Nombre
                    }).ToList();
                }
            }
            catch { }

            return View(model);
        }
    }
}
