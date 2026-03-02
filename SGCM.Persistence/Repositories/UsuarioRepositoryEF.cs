using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository;
using SGCM.Persistence.Context;

namespace SGCM.Persistence.Repositories
{
    public sealed class UsuarioRepositoryEF : IUsuarioRepository, IUnitOfWorkRepository
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly SGCMContext _context;

        public UsuarioRepositoryEF(IUnitOfWork unitOfWork, SGCMContext context)
        {
            _unitOfWork = unitOfWork;
            _context = context;
        }

        #region Metodos de Registro

        public Task ActualizarAsync(Usuario entidad)
        {
            _unitOfWork.RegistrarAmended(entidad, this);
            return Task.CompletedTask;
        }


        public Task AgregarAsync(Usuario entidad)
        {
            _unitOfWork.RegistrarNuevo(entidad, this);
            return Task.CompletedTask;
        }

        public async Task EliminarAsync(int id)
        {
            var usuario = await ObtenerPorIdAsync(id);
            if(usuario == null)
            {
                throw new ExcepcionNoEncontrado("Usuario", id);
            }
            _unitOfWork.RegistrarEliminado(usuario, this);

        }

        #endregion


        #region Metodos de Consulta

        public async Task<Usuario?> ObtenerPorEmailAsync(string email) =>
            await _context.Usuarios.FirstOrDefaultAsync(u => u.Email == email);
        
        public async Task<Usuario?> ObtenerPorIdAsync(int id) =>
             await _context.Usuarios.FindAsync(id);
        

        public async Task<IEnumerable<Usuario>> ObtenerPorRolAsync(string rol) =>
            await _context.Usuarios.Where(u => u.Rol == rol).ToListAsync();
        

        public async Task<IEnumerable<Usuario>> ObtenerTodosAsync() =>
             await _context.Usuarios.ToListAsync();



        #endregion


        #region Metodos de Persistencia

        public void PersistirCreacion(IAggregateRoot entity)
        {
            _context.Usuarios.Add((Usuario)entity);
        }

        public void PersistirEliminacion(IAggregateRoot entity)
        {
           _context.Usuarios.Remove((Usuario)entity);
        }

        public void PersistirModificacion(IAggregateRoot entity)
        {
            _context.Usuarios.Update((Usuario)entity);
        }

        #endregion



    }
}
