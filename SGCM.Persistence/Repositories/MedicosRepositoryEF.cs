using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class MedicosRepositoryEF : IMedicoRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;
        public MedicosRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;   
            _context = context;
        }

        #region Metodos de Registro
        public Task ActualizarAsync(Medico entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(Medico entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var medico = await ObtenerPorIdAsync(id);
            if (medico == null)
                throw new ExcepcionNoEncontrado("Medico", id);
            _unitOfWork.RegistrarEliminado(medico, this);
        }
        #endregion

        #region Metodos de Consulta
        public async Task<Medico?> ObtenerPorIdAsync(int id) =>
            await _context.Medicos.FindAsync(id);

        public async Task<IEnumerable<Medico>> ObtenerTodosAsync() =>
            await _context.Medicos.ToListAsync();

        public async Task<Medico?> ObtenerPorExequaturAsync(string exequatur) =>
            await _context.Medicos.FirstOrDefaultAsync(m => m.Exequatur == exequatur);

        public async Task<Medico?> ObtenerPorUsuarioIdAsync(int usuarioId) =>
            await _context.Medicos.FirstOrDefaultAsync(m => m.UsuarioId == usuarioId);

        public async Task<IEnumerable<Medico>> ObtenerPorEspecialidadAsync(int especialidadId) =>
            await _context.Medicos.Where(m => m.EspecialidadId == especialidadId).ToListAsync();
        #endregion

        #region Metodos de Persistencia
        public void PersistirCreacion(IAggregateRoot entity) =>
            _context.Medicos.Add((Medico)entity);

        public void PersistirModificacion(IAggregateRoot entity) =>
            _context.Medicos.Update((Medico)entity);

        public void PersistirEliminacion(IAggregateRoot entity) =>
            _context.Medicos.Remove((Medico)entity);
        #endregion
    }
}
