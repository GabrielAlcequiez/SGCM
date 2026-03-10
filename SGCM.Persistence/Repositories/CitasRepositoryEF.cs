using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class CitasRepositoryEF : ICitaRepository
    {

        private readonly SGCMContext _context;
        public CitasRepositoryEF(SGCMContext context) =>
            _context = context;


        #region Metodos de Registro
        public Task ActualizarAsync(Citas entidad)
        {
            _context.Citas.Update(entidad);
            return Task.CompletedTask;
        }
        public Task AgregarAsync(Citas entidad)
        {
            _context.Citas.Add(entidad);
            return Task.CompletedTask;
        }
        public async Task EliminarAsync(int id)
        {
            var cita = await ObtenerPorIdAsync(id);
            if (cita is null)
                throw new ExcepcionNoEncontrado("Citas", id);
            _context.Citas.Remove(cita);
        }
        #endregion

        #region Metodos de Consulta
        public async Task<Citas?> ObtenerPorIdAsync(int id) =>
            await _context.Citas.FindAsync(id);

        public async Task<IEnumerable<Citas>> ObtenerTodosAsync() =>
            await _context.Citas.ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorFechaAsync(DateTime fecha) =>
            await _context.Citas.Where(c => c.FechaHora.Date == fecha.Date).ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorMedicoAsync(int medicoId) =>
            await _context.Citas.Where(c => c.MedicoId == medicoId).ToListAsync();

        public async Task<IEnumerable<Citas>> ObtenerPorPacienteAsync(int pacienteId) =>
            await _context.Citas.Where(c => c.PacienteId == pacienteId).ToListAsync();

        public async Task<bool> ExisteCitaActivaPorMedicoAsync(int medicoId)
        {
            return await _context.Citas
                .AnyAsync(c => c.MedicoId == medicoId && (c.Estado == 1 || c.Estado == 2));
        }

        public async Task<bool> ExisteCitaActivaPorPacienteAsync(int pacienteId)
        {
            return await _context.Citas
                .AnyAsync(c => c.PacienteId == pacienteId && (c.Estado == 1 || c.Estado == 2));
        }

        public async Task<bool> ExisteCitaEnDiaSemanaAsync(int medicoId, int diaSemana)
        {
            return await _context.Citas
                .AnyAsync(c => c.MedicoId == medicoId
                          && (int)c.FechaHora.DayOfWeek == diaSemana
                          && c.Estado != 3);
        }
        #endregion
    }
}
