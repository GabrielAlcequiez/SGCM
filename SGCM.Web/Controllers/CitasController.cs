using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.ViewModels.Citas_Agenda.Cita;
using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Controllers
{
    public class CitasController : Controller
    {
        private readonly ICitasApiService _citasApiService;
        private readonly IMedicoApiService _medicoApiService;

        public CitasController(ICitasApiService citasApiService, IMedicoApiService medicoApiService)
        {
            _citasApiService = citasApiService;
            _medicoApiService = medicoApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            try
            {
                var citas = await _citasApiService.GetAllAsync(token);
                return View(citas);
            }
            catch (Exception)
            {
                return View(new List<CitaResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var cita = await _citasApiService.GetByIdAsync(token, id);
            if (cita == null) return NotFound();

            var viewModel = new CitaViewModel
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Estado = cita.Estado,
                Motivo = cita.Motivo,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                FechaCreacion = cita.FechaCreacion
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var model = new CrearCitaViewModel();

            try
            {
                var medicos = await _medicoApiService.GetAllAsync(token);
                model.MedicosDisponibles = medicos.Select(m => new MedicoDropdownViewModel
                {
                    Id = m.Id,
                    NombreCompleto = $"{m.Nombre} {m.Apellido}"
                }).ToList();
            }
            catch
            {
                model.MedicosDisponibles = new List<MedicoDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearCitaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearCitaDto
                {
                    MedicoId = model.MedicoId,
                    FechaHora = model.FechaHora,
                    Motivo = model.Motivo
                };

                var resultado = await _citasApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear la cita.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API.");
            }

            try
            {
                var token = GetToken();
                var medicos = await _medicoApiService.GetAllAsync(token);
                model.MedicosDisponibles = medicos.Select(m => new MedicoDropdownViewModel
                {
                    Id = m.Id,
                    NombreCompleto = $"{m.Nombre} {m.Apellido}"
                }).ToList();
            }
            catch { }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Edit(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var cita = await _citasApiService.GetByIdAsync(token, id);
            if (cita == null) return NotFound();

            var model = new EditarCitaViewModel
            {
                Id = cita.Id,
                MedicoId = cita.MedicoId,
                PacienteId = cita.PacienteId,
                FechaHora = cita.FechaHora,
                Motivo = cita.Motivo
            };

            try
            {
                var medicos = await _medicoApiService.GetAllAsync(token);
                model.MedicosDisponibles = medicos.Select(m => new MedicoDropdownViewModel
                {
                    Id = m.Id,
                    NombreCompleto = $"{m.Nombre} {m.Apellido}"
                }).ToList();
            }
            catch
            {
                model.MedicosDisponibles = new List<MedicoDropdownViewModel>();
            }

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarCitaViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearCitaDto
                {
                    MedicoId = model.MedicoId,
                    FechaHora = model.FechaHora,
                    Motivo = model.Motivo
                };

                var resultado = await _citasApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar la cita.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Error al conectar con la API.");
            }

            try
            {
                var token = GetToken();
                var medicos = await _medicoApiService.GetAllAsync(token);
                model.MedicosDisponibles = medicos.Select(m => new MedicoDropdownViewModel
                {
                    Id = m.Id,
                    NombreCompleto = $"{m.Nombre} {m.Apellido}"
                }).ToList();
            }
            catch { }

            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Delete(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var cita = await _citasApiService.GetByIdAsync(token, id);
            if (cita == null) return NotFound();

            var viewModel = new CitaViewModel
            {
                Id = cita.Id,
                FechaHora = cita.FechaHora,
                Estado = cita.Estado,
                Motivo = cita.Motivo,
                PacienteId = cita.PacienteId,
                MedicoId = cita.MedicoId,
                FechaCreacion = cita.FechaCreacion
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _citasApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Cancelar(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            try
            {
                await _citasApiService.CancelarAsync(token, id);
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public async Task<IActionResult> Completar(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            try
            {
                await _citasApiService.CompletarAsync(token, id);
            }
            catch { }

            return RedirectToAction(nameof(Index));
        }
    }
}
