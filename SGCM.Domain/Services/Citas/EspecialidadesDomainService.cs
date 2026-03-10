using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Domain.Services
{
    public class EspecialidadDomainService : IEspecialidadDomainService
    {
        private readonly IEspecialidadesRepository _especialidadesRepository;
        private readonly IMedicoRepository _medicoRepository;

        public EspecialidadDomainService(IEspecialidadesRepository especialidadesRepository, IMedicoRepository medicoRepository)
        {
            _especialidadesRepository = especialidadesRepository;
            _medicoRepository = medicoRepository;
        }

        public async Task EsNombreUnicoAsync(string nombre)
        {
            var especialidadExistente = await _especialidadesRepository.ObtenerPorNombreAsync(nombre);

            if (especialidadExistente != null)
            {
                throw new ExcepcionReglaNegocio(
                    $"Ya existe una especialidad médica registrada con el nombre '{nombre}'.",
                    "ESPECIALIDAD_DUPLICADA"
                );
            }
        }

        public async Task PuedeEliminarEspecialidadAsync(int id)
        {
            var especialidad = await _especialidadesRepository.ObtenerPorIdAsync(id);
            if (especialidad is null)
                throw new ExcepcionNoEncontrado("Especialidades", id);

            bool tieneMedicosAsociacdos = await _medicoRepository.ExisteMedicoConEspecialidadAsync(id);

            if (tieneMedicosAsociacdos)
            {
                throw new ExcepcionReglaNegocio(
                    $"No se puede eliminar la especialidad '{especialidad.Nombre}' porque tiene médicos asociados.",
                    "ESPECIALIDAD_CON_MEDICOS"
                );
            }
        }
    }
}
