using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class ProveedoresRepositoryEF : IProveedoresRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;

        public ProveedoresRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

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
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Proveedores entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var proveedor = await ObtenerPorIdAsync(id);
            if (proveedor == null)
            {
                throw new ExcepcionNoEncontrado("Proveedores", id);
            }
            _unitOfWork.RegistrarEliminado(proveedor, this);
        }

        #endregion


        #region Metodos de Persistencia

        public void PersistirCreacion(IAggregateRoot entity)
        {
            _context.Proveedores.Add((Proveedores)entity);
        }

        public void PersistirEliminacion(IAggregateRoot entity)
        {
            _context.Proveedores.Remove((Proveedores)entity);
        }

        public void PersistirModificacion(IAggregateRoot entity)
        {
            _context.Proveedores.Update((Proveedores)entity);
        }

        #endregion

    }
}
