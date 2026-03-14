using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class UsuarioRepositoryEF : IUsuarioRepository
    {
        private readonly SGCMContext _context;

        public UsuarioRepositoryEF(SGCMContext context) =>
            _context = context;
        

        #region Metodos de Registro

        public Task ActualizarAsync(Usuario entidad)
        {
            _context.Usuarios.Update(entidad);
            return Task.CompletedTask;
        }


        public Task AgregarAsync(Usuario entidad)
        {
            _context.Usuarios.Add(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await ObtenerPorIdAsync(id);
            if(usuario is null)
            {
                throw new ExcepcionNoEncontrado("Usuario", id);
            }
            usuario.Eliminar();
        }

        #endregion


        #region Metodos de Consulta

        public async Task<Usuario?> ObtenerPorEmailAsync(string email) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        
        public async Task<Usuario?> ObtenerPorIdAsync(int id) =>
             await _context.Usuarios.FindAsync(id);
        

        public async Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol) =>
            await _context.Usuarios.Where(u => u.Rol == rol).ToListAsync();
        

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync() =>
             await _context.Usuarios.ToListAsync();



        #endregion
    }
}
