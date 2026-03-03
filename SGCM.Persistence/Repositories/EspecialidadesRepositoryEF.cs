using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class EspecialidadesRepositoryEF : IEspecialidadesRepository
    {
        private readonly SGCMContext _context;
        public EspecialidadesRepositoryEF(SGCMContext context) =>
            _context = context;
        

        #region Metodos de Registro
        public Task ActualizarAsync(Especialidades entidad)
        {
            _context.Especialidades.Update(entidad);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Especialidades entidad)
        {
           _context.Especialidades.Add(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var especialidad = await _context.Especialidades.FindAsync(id);
            if (especialidad is null)
                throw new ExcepcionNoEncontrado("Especialidades", id);
            _context.Especialidades.Remove(especialidad);
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

    }
}
