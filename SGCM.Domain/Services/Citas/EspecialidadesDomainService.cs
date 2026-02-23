using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Domain.Services
{
    public class EspecialidadDomainService : IEspecialidadDomainService
    {
        private readonly IEspecialidadesRepository _especialidadesRepository;

        public EspecialidadDomainService(IEspecialidadesRepository especialidadesRepository)
        {
            _especialidadesRepository = especialidadesRepository;
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
    }
}
