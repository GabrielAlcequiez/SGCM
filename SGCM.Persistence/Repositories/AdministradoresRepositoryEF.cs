using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class AdministradoresRepositoryEF : IAdministradoresRepository
    {
        private readonly SGCMContext _context;
        public AdministradoresRepositoryEF(SGCMContext context) => _context = context;
        
        #region Metodos de Registro

        public Task ActualizarAsync(Administradores entidad)
        {
            _context.Administradores.Update(entidad);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Administradores entidad)
        {
            _context.Administradores.Add(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var admin = await ObtenerPorIdAsync(id);
            if (admin is null)
            {
                throw new ExcepcionNoEncontrado("Administradores", id);
            }
            admin.Eliminar();
        }


        #endregion


        #region Metodos de Consulta

        public async Task<Administradores?> ObtenerPorIdAsync(int id) =>
            await _context.Administradores.FindAsync(id);
        public async Task<IEnumerable<Administradores>> ObtenerTodosAsync() =>
            await _context.Administradores.ToListAsync();

        #endregion
    }
}
