using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.ViewModels.Seguridad_Usuario.Auditoria;
using SGCM.Web.Services.Seguridad_Usuarios;

namespace SGCM.Web.Controllers
{
    public class AuditoriaController : Controller
    {
        private readonly IAuditoriaApiService _auditoriaApiService;
        private readonly IUsuarioApiService _usuarioApiService;

        public AuditoriaController(IAuditoriaApiService auditoriaApiService, IUsuarioApiService usuarioApiService)
        {
            _auditoriaApiService = auditoriaApiService;
            _usuarioApiService = usuarioApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var logs = await _auditoriaApiService.GetAllAsync(token);
            return View(logs);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var logs = await _auditoriaApiService.GetAllAsync(token);
            var log = logs.FirstOrDefault(l => l.Id == id);
            if (log == null) return NotFound();

            var viewModel = new AuditoriaViewModel
            {
                Id = log.Id,
                UsuarioId = log.UsuarioId,
                Accion = log.Accion,
                EntidadAfectada = log.EntidadAfectada,
                Fecha = log.Fecha,
                Detalles = log.Detalles
            };
            return View(viewModel);
        }

        [HttpGet]
        public async Task<IActionResult> PorUsuario(int usuarioId)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var logs = await _auditoriaApiService.GetByUsuarioAsync(token, usuarioId);
            return View("Index", logs);
        }

        [HttpGet]
        public async Task<IActionResult> PorRangoFechas(DateTime? inicio, DateTime? fin)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            if (inicio.HasValue && fin.HasValue)
            {
                var logs = await _auditoriaApiService.GetByRangoFechasAsync(token, inicio.Value, fin.Value);
                return View("Index", logs);
            }

            var todos = await _auditoriaApiService.GetAllAsync(token);
            return View("Index", todos);
        }
    }
}
