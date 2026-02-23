using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Services.Interfaces.IPacientes;

namespace SGCM.Domain.Services
{
    public class ProveedoresDomainService : IProveedoresDomainService
    {
        private readonly IProveedoresRepository _proveedoresRepository;

        public ProveedoresDomainService(IProveedoresRepository proveedoresRepository)
        {
            _proveedoresRepository = proveedoresRepository;
        }

        public async Task EsRNCUnicoAsync(string rnc)
        {
            var proveedorExistente = await _proveedoresRepository.ObtenerPorRNCAsync(rnc);

            if (proveedorExistente != null)
            {
                throw new ExcepcionReglaNegocio(
                    $"El RNC '{rnc}' ya se encuentra registrado para otro proveedor.",
                    "RNC_PROVEEDOR_DUPLICADO"
                );
            }
        }

        public async Task ValidarNombreUnicoAsync(string nombre)
        {
            var proveedorExistente = await _proveedoresRepository.ObtenerPorNombreAsync(nombre);

            if (proveedorExistente != null)
            {
                throw new ExcepcionReglaNegocio(
                    $"Ya existe un proveedor o ARS registrado con el nombre '{nombre}'.",
                    "NOMBRE_PROVEEDOR_DUPLICADO"
                );
            }
        }
    }
}
