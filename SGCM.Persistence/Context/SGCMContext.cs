using Microsoft.EntityFrameworkCore;
using SGCM.Domain.Entities.Citas_Agenda;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Entities.Seguridad_Usuarios;

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

    }
}
