using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class PacienteRepositoryEF : IPacienteRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;

        public PacienteRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Metodos de Consulta
        public async Task<IEnumerable<Paciente>> ObtenerPorApellidoAsync(string apellido) => await _context.Pacientes.Where(p => p.Apellido.Contains(apellido)).ToListAsync();

        public async Task<Paciente?> ObtenerPorIdAsync(int id) => await _context.Pacientes.FindAsync(id);

        public async Task<IEnumerable<Paciente>> ObtenerPorNombreAsync(string nombre) => await _context.Pacientes.Where(p => p.Nombre.Contains(nombre)).ToListAsync();

        public async Task<IEnumerable<Paciente>> ObtenerPorTelefonoAsync(string telefono) => await _context.Pacientes.Where(p => p.Telefono == telefono).ToListAsync();

        public async Task<IEnumerable<Paciente>> ObtenerTodosAsync() => await _context.Pacientes.ToListAsync();
        #endregion

        #region Metodos de Registro

        public Task ActualizarAsync(Paciente entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }

        public Task AgregarAsync(Paciente entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            // Pendiente de implementar soft delete (hard delete actualmente)
            var paciente = await ObtenerPorIdAsync(id);
            if (paciente == null)
            {
                throw new ExcepcionNoEncontrado("Paciente", id);
            }
            _unitOfWork.RegistrarEliminado(paciente, this);
        }


        #endregion

        #region Metodos de Persistencia

        public void PersistirCreacion(IAggregateRoot entity)
        {
            _context.Pacientes.Add((Paciente)entity);
        }

        public void PersistirEliminacion(IAggregateRoot entity)
        {
            _context.Pacientes.Remove((Paciente)entity);
        }

        public void PersistirModificacion(IAggregateRoot entity)
        {
            _context.Pacientes.Update((Paciente)entity);
        }


        #endregion
    
    }
}
