using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class AdministradoresRepositoryEF : IAdministradoresRepository, IUnitOfWorkRepository
    {
        private readonly SGCMContext _context;
        private readonly IUnitOfWork _unitOfWork;
        public AdministradoresRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }


        #region Metodos de Registro

        public Task ActualizarAsync(Administradores entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Administradores entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var admin = await ObtenerPorIdAsync(id);
            if (admin == null)
            {
                throw new ExcepcionNoEncontrado("Administradores", id);
            }
            _unitOfWork.RegistrarEliminado(admin, this);
        }


        #endregion


        #region Metodos de Consulta

        public async Task<Administradores?> ObtenerPorIdAsync(int id) =>
            await _context.Administradores.FindAsync(id);
        public async Task<IEnumerable<Administradores>> ObtenerTodosAsync() =>
            await _context.Administradores.ToListAsync();



        #endregion


        #region Metodos de Persistencia

        public void PersistirCreacion(IAggregateRoot entity)=>
            _context.Administradores.Add((Administradores)entity);

        public void PersistirEliminacion(IAggregateRoot entity) =>
            _context.Administradores.Remove((Administradores)entity);
        

        public void PersistirModificacion(IAggregateRoot entity)=>
            _context.Administradores.Update((Administradores)entity);

        #endregion
    }
}
