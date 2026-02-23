using SGCM.Domain.Repository;

namespace SGCM.Domain.Entities.Pacientes
{
    public interface IPacienteRepository : IBaseRepository<Paciente>
    {
            Task<IEnumerable<Paciente>> ObtenerPorNombreAsync(string nombre);
            Task<IEnumerable<Paciente>> ObtenerPorApellidoAsync(string apellido);
            Task<IEnumerable<Paciente>> ObtenerPorTelefonoAsync(string telefono);
    }
}
