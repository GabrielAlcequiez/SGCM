using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.ViewModels.Citas_Agenda.Medico;
using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Controllers
{
    public class MedicosController : Controller
    {
        private readonly IMedicoApiService _medicoApiService;
        private readonly IEspecialidadesApiService _especialidadesApiService;

        public MedicosController(IMedicoApiService medicoApiService, IEspecialidadesApiService especialidadesApiService)
        {
            _medicoApiService = medicoApiService;
            _especialidadesApiService = especialidadesApiService;
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
    }
}
