using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services.Citas_Agenda;
using SGCM.Web.ViewModels.Citas_Agenda.Especialidades;
using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Web.Controllers
{
    public class EspecialidadesController : Controller
    {
        private readonly IEspecialidadesApiService _especialidadesApiService;

        public EspecialidadesController(IEspecialidadesApiService especialidadesApiService)
        {
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
                var especialidades = await _especialidadesApiService.GetAllAsync(token);
                return View(especialidades);
            }
            catch (Exception)
            {
                return View(new List<EspecialidadesResponseDto>());
            }
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var especialidad = await _especialidadesApiService.GetByIdAsync(token, id);
            if (especialidad == null) return NotFound();

            var viewModel = new EspecialidadesViewModel
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View(new CrearEspecialidadesViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearEspecialidadesViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearEspecialidadesDto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion
                };

                var resultado = await _especialidadesApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear la especialidad.");
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

            var especialidad = await _especialidadesApiService.GetByIdAsync(token, id);
            if (especialidad == null) return NotFound();

            var model = new EditarEspecialidadesViewModel
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarEspecialidadesViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearEspecialidadesDto
                {
                    Nombre = model.Nombre,
                    Descripcion = model.Descripcion
                };

                var resultado = await _especialidadesApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar la especialidad.");
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

            var especialidad = await _especialidadesApiService.GetByIdAsync(token, id);
            if (especialidad == null) return NotFound();

            var viewModel = new EspecialidadesViewModel
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _especialidadesApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }
    }
}
