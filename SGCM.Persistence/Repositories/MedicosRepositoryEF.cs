using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class MedicosRepositoryEF : IMedicoRepository
    {
        private readonly SGCMContext _context;
        public MedicosRepositoryEF(SGCMContext context) => _context = context;


        #region Metodos de Registro
        public Task ActualizarAsync(Medico entidad)
        {
            _context.Medicos.Update(entidad);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(Medico entidad)
        {
            _context.Medicos.Add(entidad);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var medico = await ObtenerPorIdAsync(id);
            if (medico is null)
                throw new ExcepcionNoEncontrado("Medico", id);
            medico.Eliminar();
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

        public async Task<bool> ExisteMedicoConEspecialidadAsync(int especialidadId)
        {
            return await _context.Medicos.AnyAsync(m => m.EspecialidadId == especialidadId);
        }
        #endregion
    }
}
