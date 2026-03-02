using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class CitasRepositoryEF : ICitaRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;
        public CitasRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _context = context;
            _unitOfWork = unitOfWork;
        }

        #region Metodos de Registro
        public Task ActualizarAsync(Citas entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(Citas entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var cita = await ObtenerPorIdAsync(id);
            if (cita == null)
                throw new ExcepcionNoEncontrado("Citas", id);
            _unitOfWork.RegistrarEliminado(cita, this);
        }
        #endregion

        #region Metodos de Consulta
        public async Task<Citas?> ObtenerPorIdAsync(int id) =>
            await _context.Citas.FindAsync(id);

        public async Task<IEnumerable<Citas>> ObtenerTodosAsync() =>
            await _context.Citas.ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorFechaAsync(DateTime fecha) =>
            await _context.Citas.Where(c => c.FechaCreacion.Date == fecha.Date).ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorMedicoAsync(int medicoId) =>
            await _context.Citas.Where(c => c.MedicoId == medicoId).ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorPacienteAsync(int pacienteId) =>
            await _context.Citas.Where(c => c.PacienteId == pacienteId).ToListAsync();
        #endregion

        #region Metodos de Persistencia
        public void PersistirCreacion(IAggregateRoot entity) =>
            _context.Citas.Add((Citas)entity);

        public void PersistirModificacion(IAggregateRoot entity) =>
            _context.Citas.Update((Citas)entity);

        public void PersistirEliminacion(IAggregateRoot entity) =>
            _context.Citas.Remove((Citas)entity);
        #endregion
    }
}
