using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.Services.Auth;
using SGCM.Web.ViewModels.Citas_Agenda.Medico;
using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Dtos.Login;

namespace SGCM.Web.Controllers
{
    public class MedicosController : Controller
    {
        private readonly IMedicoApiService _medicoApiService;
        private readonly IEspecialidadesApiService _especialidadesApiService;
        private readonly IAuthApiService _authApiService;

        public MedicosController(
            IMedicoApiService medicoApiService,
            IEspecialidadesApiService especialidadesApiService,
            IAuthApiService authApiService)
        {
            _medicoApiService = medicoApiService;
            _especialidadesApiService = especialidadesApiService;
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
                var medicos = await _medicoApiService.GetAllAsync(token);
                return View(medicos);
            }
            catch (Exception)
            {
                return View(new List<MedicoResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var medico = await _medicoApiService.GetByIdAsync(token, id);
            if (medico == null) return NotFound();

            var viewModel = new MedicoViewModel
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono,
                EspecialidadId = medico.EspecialidadId,
                UsuarioId = medico.UsuarioId
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var model = new CrearMedicoViewModel();
            try
            {
                var especialidades = await _especialidadesApiService.GetAllAsync(token);
                model.EspecialidadesDisponibles = especialidades.Select(e => new EspecialidadDropdownViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToList();
            }
            catch
            {
                model.EspecialidadesDisponibles = new List<EspecialidadDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearMedicoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearMedicoDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    EspecialidadId = model.EspecialidadId,
                    Exequatur = model.Exequatur,
                    Telefono = model.Telefono
                };

                var resultado = await _medicoApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear el médico.");
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

            var medico = await _medicoApiService.GetByIdAsync(token, id);
            if (medico == null) return NotFound();

            var model = new EditarMedicoViewModel
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                EspecialidadId = medico.EspecialidadId,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono
            };

            try
            {
                var especialidades = await _especialidadesApiService.GetAllAsync(token);
                model.EspecialidadesDisponibles = especialidades.Select(e => new EspecialidadDropdownViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToList();
            }
            catch
            {
                model.EspecialidadesDisponibles = new List<EspecialidadDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarMedicoViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearMedicoDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    EspecialidadId = model.EspecialidadId,
                    Exequatur = model.Exequatur,
                    Telefono = model.Telefono
                };

                var resultado = await _medicoApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar el médico.");
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

            var medico = await _medicoApiService.GetByIdAsync(token, id);
            if (medico == null) return NotFound();

            var viewModel = new MedicoViewModel
            {
                Id = medico.Id,
                Nombre = medico.Nombre,
                Apellido = medico.Apellido,
                Exequatur = medico.Exequatur,
                Telefono = medico.Telefono,
                EspecialidadId = medico.EspecialidadId,
                UsuarioId = medico.UsuarioId
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _medicoApiService.DeleteAsync(token, id);
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

            if (HttpContext.Session.GetInt32("MedicoId") != null)
            {
                return RedirectToAction("Index", "Home");
            }

            var model = new CompletarPerfilMedicoViewModel();

            try
            {
                var token = GetToken();
                var especialidades = _especialidadesApiService.GetAllAsync(token).Result;
                model.EspecialidadesDisponibles = especialidades.Select(e => new EspecialidadDropdownViewModel
                {
                    Id = e.Id,
                    Nombre = e.Nombre
                }).ToList();
            }
            catch
            {
                model.EspecialidadesDisponibles = new List<EspecialidadDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CompletarPerfil(CompletarPerfilMedicoViewModel model)
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
                        var especialidades = await _especialidadesApiService.GetAllAsync(token);
                        model.EspecialidadesDisponibles = especialidades.Select(e => new EspecialidadDropdownViewModel
                        {
                            Id = e.Id,
                            Nombre = e.Nombre
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

                var dto = new CrearMedicoDto
                {
                    Nombre = model.Nombre,
                    Apellido = model.Apellido,
                    Telefono = model.Telefono,
                    EspecialidadId = model.EspecialidadId ?? 0,
                    Exequatur = model.Exequatur
                };

                var medicoResult = await _medicoApiService.CreateAsync(token, dto);

                if (medicoResult != null)
                {
                    HttpContext.Session.SetInt32("MedicoId", medicoResult.Id);
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

            try
            {
                var token = GetToken();
                if (!string.IsNullOrEmpty(token))
                {
                    var especialidades = await _especialidadesApiService.GetAllAsync(token);
                    model.EspecialidadesDisponibles = especialidades.Select(e => new EspecialidadDropdownViewModel
                    {
                        Id = e.Id,
                        Nombre = e.Nombre
                    }).ToList();
                }
            }
            catch { }

            return View(model);
        }
    }
}
