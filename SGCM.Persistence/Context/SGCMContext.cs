using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Base;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Entities.Seguridad_Usuarios;
using System.Linq.Expressions;

namespace SGCM.Persistence.Context
{
    public sealed class SGCMContext : DbContext
    {
        public SGCMContext(DbContextOptions<SGCMContext> options) : base(options) { }
        public DbSet<Usuario> Usuarios { get; set; }
        public DbSet<AuditoriaLogs> AuditoriaLogs { get; set; }
        public DbSet<Administradores> Administradores { get; set; }
        public DbSet<Paciente> Pacientes { get; set; }
        public DbSet<Proveedores> Proveedores { get; set; }
        public DbSet<Citas> Citas { get; set; }
        public DbSet<Disponibilidad> Disponibilidad { get; set; }
        public DbSet<Medico> Medicos { get; set; }
        public DbSet<Especialidades> Especialidades { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            foreach (var entityType in modelBuilder.Model.GetEntityTypes())
            {
                if (typeof(DeletableEntity).IsAssignableFrom(entityType.ClrType))
                {
                    modelBuilder.Entity(entityType.ClrType)
                                .HasQueryFilter(ConvertirFiltroExpresion(entityType.ClrType));
                }
            }
        }
        private static LambdaExpression ConvertirFiltroExpresion(Type type)
        {
            var parameter = Expression.Parameter(type, "x");
            var property = Expression.Property(parameter, nameof(DeletableEntity.EstaEliminado));
            var notExpression = Expression.Not(property);
            return Expression.Lambda(notExpression, parameter);
        }
    }

}