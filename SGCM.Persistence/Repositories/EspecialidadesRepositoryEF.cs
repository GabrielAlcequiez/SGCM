using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;
using System.Data;

namespace SGCM.Persistence.Repositories
{
    public sealed class EspecialidadesRepositoryEF : IEspecialidadesRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;
        public EspecialidadesRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Metodos de Registro
        public Task ActualizarAsync(Especialidades entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Especialidades entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad == null)
                throw new ExcepcionNoEncontrado("Especialidades", id);
            _unitOfWork.RegistrarEliminado(especialidad, this);
        }

        #endregion

        #region Metodos de Consulta

        public async Task<Especialidades?> ObtenerPorIdAsync(int id) =>
            await _context.Especialidades.FindAsync(id);

        public async Task<Especialidades?> ObtenerPorNombreAsync(string nombre) =>
            await _context.Especialidades.FirstOrDefaultAsync(e => e.Nombre == nombre);


        public async Task<IEnumerable<Especialidades>> ObtenerTodosAsync() =>
           await _context.Especialidades.ToListAsync();

        #endregion


        #region Metodos de Persistencia

        public void PersistirCreacion(IAggregateRoot entity)
        {
            _context.Especialidades.Add((Especialidades)entity);
        }

        public void PersistirEliminacion(IAggregateRoot entity)
        {
            _context.Especialidades.Remove((Especialidades)entity);
        }

        public void PersistirModificacion(IAggregateRoot entity)
        {
            _context.Especialidades.Update((Especialidades)entity);
        }


        #endregion
    }
}
