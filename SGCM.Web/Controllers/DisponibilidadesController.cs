using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.ViewModels.Citas_Agenda.Disponibilidad;
using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Controllers
{
    public class DisponibilidadesController : Controller
    {
        private readonly IDisponibilidadApiService _disponibilidadApiService;
        private readonly IMedicoApiService _medicoApiService;

        public DisponibilidadesController(IDisponibilidadApiService disponibilidadApiService, IMedicoApiService medicoApiService)
        {
            _disponibilidadApiService = disponibilidadApiService;
            _medicoApiService = medicoApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        private static List<DiaSemanaOption> GetDiasSemana()
        {
            return new List<DiaSemanaOption>
            {
                new() { Valor = 0, Nombre = "Domingo" },
                new() { Valor = 1, Nombre = "Lunes" },
                new() { Valor = 2, Nombre = "Martes" },
                new() { Valor = 3, Nombre = "Miércoles" },
                new() { Valor = 4, Nombre = "Jueves" },
                new() { Valor = 5, Nombre = "Viernes" },
                new() { Valor = 6, Nombre = "Sábado" }
            };
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            try
            {
                var disponibilidades = await _disponibilidadApiService.GetAllAsync(token);
                return View(disponibilidades);
            }
            catch (Exception)
            {
                return View(new List<DisponibilidadResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var disponibilidad = await _disponibilidadApiService.GetByIdAsync(token, id);
            if (disponibilidad == null) return NotFound();

            var viewModel = new DisponibilidadViewModel
            {
                Id = disponibilidad.Id,
                MedicoId = disponibilidad.MedicoId,
                DiaSemana = disponibilidad.DiaSemana,
                HoraInicio = disponibilidad.HoraInicio,
                HoraFin = disponibilidad.HoraFin,
                EsDiaLibre = disponibilidad.EsDiaLibre
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var model = new CrearDisponibilidadViewModel
            {
                DiasSemana = GetDiasSemana()
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
        public async Task<IActionResult> Create(CrearDisponibilidadViewModel model)
        {
            model.DiasSemana = GetDiasSemana();
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearDisponibilidadDto
                {
                    DiaSemana = model.DiaSemana,
                    HoraInicio = model.HoraInicio,
                    HoraFin = model.HoraFin,
                    EsDiaLibre = model.EsDiaLibre
                };

                var resultado = await _disponibilidadApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear la disponibilidad.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor intente más tarde.");
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

            var disponibilidad = await _disponibilidadApiService.GetByIdAsync(token, id);
            if (disponibilidad == null) return NotFound();

            var model = new EditarDisponibilidadViewModel
            {
                Id = disponibilidad.Id,
                MedicoId = disponibilidad.MedicoId,
                DiaSemana = disponibilidad.DiaSemana,
                HoraInicio = disponibilidad.HoraInicio,
                HoraFin = disponibilidad.HoraFin,
                EsDiaLibre = disponibilidad.EsDiaLibre,
                DiasSemana = GetDiasSemana()
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
        public async Task<IActionResult> Edit(int id, EditarDisponibilidadViewModel model)
        {
            model.DiasSemana = GetDiasSemana();
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearDisponibilidadDto
                {
                    DiaSemana = model.DiaSemana,
                    HoraInicio = model.HoraInicio,
                    HoraFin = model.HoraFin,
                    EsDiaLibre = model.EsDiaLibre
                };

                var resultado = await _disponibilidadApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar la disponibilidad.");
            }
            catch (HttpRequestException)
            {
                ModelState.AddModelError(string.Empty, "No se pudo conectar con el servidor. Verifique que la API esté ejecutándose.");
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "Ocurrió un error inesperado. Por favor intente más tarde.");
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

            var disponibilidad = await _disponibilidadApiService.GetByIdAsync(token, id);
            if (disponibilidad == null) return NotFound();

            var viewModel = new DisponibilidadViewModel
            {
                Id = disponibilidad.Id,
                MedicoId = disponibilidad.MedicoId,
                DiaSemana = disponibilidad.DiaSemana,
                HoraInicio = disponibilidad.HoraInicio,
                HoraFin = disponibilidad.HoraFin,
                EsDiaLibre = disponibilidad.EsDiaLibre
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _disponibilidadApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
