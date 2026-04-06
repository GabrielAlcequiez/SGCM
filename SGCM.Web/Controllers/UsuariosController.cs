using Microsoft.AspNetCore.Mvc;
using SGCM.Web.Services;
using SGCM.Web.Services.Seguridad_Usuarios;
using SGCM.Web.ViewModels.Seguridad_Usuario.Usuario;
using SGCM.Application.Dtos.Seguridad_Usuarios;

namespace SGCM.Web.Controllers
{
    public class UsuariosController : Controller
    {
        private readonly IUsuarioApiService _usuarioApiService;

        public UsuariosController(IUsuarioApiService usuarioApiService)
        {
            _usuarioApiService = usuarioApiService;
        }

        private string GetToken() => HttpContext.Session.GetString("JWTToken") ?? string.Empty;

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var usuarios = await _usuarioApiService.GetAllAsync(token);
            return View(usuarios);
        }

        [HttpGet]
        public async Task<IActionResult> Details(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            var usuario = await _usuarioApiService.GetByIdAsync(token, id);
            if (usuario == null) return NotFound();

            var viewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
            return View(viewModel);
        }

        [HttpGet]
        public IActionResult Create()
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View(new CrearUsuarioViewModel());
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CrearUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CrearUsuarioDto
                {
                    email = model.Email,
                    password = model.Password,
                    Rol = model.Rol
                };

                var resultado = await _usuarioApiService.CreateAsync(token, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible crear el usuario.");
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

            var usuario = await _usuarioApiService.GetByIdAsync(token, id);
            if (usuario == null) return NotFound();

            var viewModel = new EditarUsuarioViewModel
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol
            };
            return View(viewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, EditarUsuarioViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new ActualizarUsuarioDto
                {
                    email = model.Email,
                    Rol = model.Rol
                };

                var resultado = await _usuarioApiService.UpdateAsync(token, id, dto);
                if (resultado != null)
                {
                    return RedirectToAction(nameof(Index));
                }
                ModelState.AddModelError(string.Empty, "No fue posible actualizar el usuario.");
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

            var usuario = await _usuarioApiService.GetByIdAsync(token, id);
            if (usuario == null) return NotFound();

            var viewModel = new UsuarioViewModel
            {
                Id = usuario.Id,
                Email = usuario.Email,
                Rol = usuario.Rol,
                FechaRegistro = usuario.FechaRegistro
            };
            return View(viewModel);
        }

        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            await _usuarioApiService.DeleteAsync(token, id);
            return RedirectToAction(nameof(Index));
        }

        [HttpGet]
        public IActionResult ChangePassword(int id)
        {
            var token = GetToken();
            if (string.IsNullOrEmpty(token)) return RedirectToAction("Login", "Auth");

            return View(new CambiarPasswordViewModel { Id = id });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> ChangePassword(CambiarPasswordViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var token = GetToken();
                var dto = new CambiarPasswordUsuarioDto
                {
                    PasswordActual = model.PasswordActual,
                    NuevoPassword = model.NuevoPassword
                };

                await _usuarioApiService.ChangePasswordAsync(token, model.Id, dto);
                return RedirectToAction(nameof(Index));
            }
            catch (Exception)
            {
                ModelState.AddModelError(string.Empty, "No fue posible cambiar la contraseña.");
            }

            return View(model);
        }
    }
}
