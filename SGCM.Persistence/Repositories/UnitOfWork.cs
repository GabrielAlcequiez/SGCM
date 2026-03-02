using SGCM.Domain.Repository;
using SGCM.Persistence.Context;
using System.Transactions;

namespace SGCM.Persistence.Repositories
{
    public class UnitOfWork : IUnitOfWork
    {
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> entidadAñadida;
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> entidadModificada;
        private Dictionary<IAggregateRoot, IUnitOfWorkRepository> entidadEliminada;

        private readonly SGCMContext _context;

        public UnitOfWork(SGCMContext context)
        {
            entidadAñadida = new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
            entidadModificada = new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
            entidadEliminada = new Dictionary<IAggregateRoot, IUnitOfWorkRepository>();
            _context = context;
        }

        public void RegistrarAmended(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!entidadModificada.ContainsKey(entity))
            {
                entidadModificada.Add(entity, repository);
            }
        }

        public void RegistrarEliminado(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!entidadEliminada.ContainsKey(entity))
            {
                entidadEliminada.Add(entity, repository);
            }
        }

        public void RegistrarNuevo(IAggregateRoot entity, IUnitOfWorkRepository repository)
        {
            if (!entidadAñadida.ContainsKey(entity))
            {
                entidadAñadida.Add(entity, repository);
            }
        }

        public async Task Commit()
        {
            using var scope = new TransactionScope(TransactionScopeAsyncFlowOption.Enabled);

            foreach (IAggregateRoot entity in this.entidadAñadida.Keys)
            {
                this.entidadAñadida[entity].PersistirCreacion(entity);
            }
            foreach (IAggregateRoot entity in this.entidadModificada.Keys)
            {
                this.entidadModificada[entity].PersistirModificacion(entity);
            }
            foreach (IAggregateRoot entity in this.entidadEliminada.Keys)
            {
                this.entidadEliminada[entity].PersistirEliminacion(entity);
            }

            await _context.SaveChangesAsync(); 
            scope.Complete();                  
            Dispose();                       
        }
        

        public void Dispose()
        {
            entidadAñadida.Clear();
            entidadModificada.Clear();
            entidadEliminada.Clear();
        }
    }
}
