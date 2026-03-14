using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class ProveedoresRepositoryEF : IProveedoresRepository
    {
        private readonly SGCMContext _context;

        public ProveedoresRepositoryEF(SGCMContext context) =>
            _context = context;
        

        #region Metodos de Consulta

        public async Task<Proveedores?> ObtenerPorIdAsync(int id) =>
            await _context.Proveedores.FindAsync(id);

        public async Task<Proveedores?> ObtenerPorNombreAsync(string nombre) =>
            await _context.Proveedores.FirstOrDefaultAsync(p => p.Nombre == nombre);
        

        public async Task<Proveedores?> ObtenerPorRNCAsync(string rnc) =>
            await _context.Proveedores.FirstOrDefaultAsync(p => p.RNC == rnc);
        

        public async Task<IEnumerable<Proveedores>> ObtenerTodosAsync() =>
            await _context.Proveedores.ToListAsync();




        #endregion

        #region Metodos de Registro

        public Task ActualizarAsync(Proveedores entidad)
        {
            _context.Proveedores.Update(entidad);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Proveedores entidad)
        {
            _context.Proveedores.Add(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var proveedor = await ObtenerPorIdAsync(id);
            if (proveedor is null)
            {
                throw new ExcepcionNoEncontrado("Proveedores", id);
            }
            proveedor.Eliminar();
        }

        #endregion

    }
}
