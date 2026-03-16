using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class PacienteRepositoryEF : IPacienteRepository
    {
        private readonly SGCMContext _context;

        public PacienteRepositoryEF(SGCMContext context) => _context = context;
        

        #region Metodos de Consulta
        public async Task<IEnumerable<Paciente>> ObtenerPorApellidoAsync(string apellido) => await _context.Pacientes.Where(p => p.Apellido.Contains(apellido)).ToListAsync();

        public async Task<Paciente?> ObtenerPorIdAsync(int id) => await _context.Pacientes.FindAsync(id);

        public async Task<IEnumerable<Paciente>> ObtenerPorNombreAsync(string nombre) => await _context.Pacientes.Where(p => p.Nombre.Contains(nombre)).ToListAsync();

        public async Task<IEnumerable<Paciente>> ObtenerPorTelefonoAsync(string telefono) => await _context.Pacientes.Where(p => p.Telefono == telefono).ToListAsync();

        public async Task<Paciente?> ObtenerPorUsuarioIdAsync(int usuarioId) => await _context.Pacientes.FirstOrDefaultAsync(p => p.UsuarioId == usuarioId);

        public async Task<IEnumerable<Paciente>> ObtenerTodosAsync() => await _context.Pacientes.ToListAsync();
        #endregion

        #region Metodos de Registro

        public Task ActualizarAsync(Paciente entidad)
        {
            _context.Pacientes.Update(entidad);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Paciente entidad)
        {
            _context.Pacientes.Add(entidad);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var paciente = await ObtenerPorIdAsync(id);
            if (paciente is null)
            {
                throw new ExcepcionNoEncontrado("Paciente", id);
            }
            paciente.Eliminar();
        }


        #endregion
    }
}
