using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class DisponibilidadRepositoryEF : IDisponibilidadRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;

        public DisponibilidadRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Metodos de Registro
        public Task ActualizarAsync(Disponibilidad entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(Disponibilidad entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var disponibilidad = await ObtenerPorIdAsync(id);
            if (disponibilidad == null)
                throw new ExcepcionNoEncontrado("Disponibilidad", id);
            _unitOfWork.RegistrarEliminado(disponibilidad, this);
        }
        #endregion

        #region Metodos de Consulta
        public async Task<Disponibilidad?> ObtenerPorIdAsync(int id) =>
            await _context.Disponibilidad.FindAsync(id);

        public async Task<IEnumerable<Disponibilidad>> ObtenerTodosAsync() =>
            await _context.Disponibilidad.ToListAsync();

        public async Task<IEnumerable<Disponibilidad>> ObtenerPorMedicoIdAsync(int medicoId) =>
            await _context.Disponibilidad.Where(d => d.MedicoId == medicoId).ToListAsync();

        public async Task<Disponibilidad?> ObtenerPorMedicoYDiaAsync(int medicoId, int diaSemana) =>
            await _context.Disponibilidad.FirstOrDefaultAsync(d => d.MedicoId == medicoId && d.DiaSemana == diaSemana);
        #endregion

        #region Metodos de Persistencia
        public void PersistirCreacion(IAggregateRoot entity) =>
            _context.Disponibilidad.Add((Disponibilidad)entity);

        public void PersistirModificacion(IAggregateRoot entity) =>
            _context.Disponibilidad.Update((Disponibilidad)entity);

        public void PersistirEliminacion(IAggregateRoot entity) =>
            _context.Disponibilidad.Remove((Disponibilidad)entity);
        #endregion
    }
}
