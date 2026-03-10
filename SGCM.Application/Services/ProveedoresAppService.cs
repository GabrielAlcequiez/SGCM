using SGCM.Application.Dtos.Pacientes;
using SGCM.Application.Interfaces;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Pacientes;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Services.Interfaces.IPacientes;

namespace SGCM.Application.Services
{
    public class ProveedoresAppService : IProveedoresAppService
    {
        private readonly IProveedoresRepository _repository;
        private readonly IAuditoriaLogger _auditoriaLogger;
        private readonly IProveedoresDomainService _domainService;
        public ProveedoresAppService(IProveedoresRepository repository, IAuditoriaLogger auditoriaLogger, IProveedoresDomainService domainService)
        {
            _repository = repository;
            _auditoriaLogger = auditoriaLogger;
            _domainService = domainService;
        }

        public async Task<ProveedoresResponseDto> CrearAsync(CrearProveedoresDto dto)
        {
            var proveedor = new Proveedores
            (
                dto.Nombre,
                dto.RNC,
                dto.Telefono,
                dto.CoberturaDefault
            );

            await _domainService.ValidarNombreUnicoAsync(proveedor.Nombre);
            await _domainService.EsRNCUnicoAsync(proveedor.RNC);

            await _repository.AgregarAsync(proveedor);

            int usuarioidtemp = 0; // Temporal hasta que se implemente la autenticación 
            await _auditoriaLogger.RegistrarAsync(usuarioidtemp, "Crear", "Proveedores", $"Proveedor creado con ID: {proveedor.Id}");
        
            return new ProveedoresResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };

        }
        public async Task<ProveedoresResponseDto> ActualizarAsync(int id, CrearProveedoresDto dtoC)
        {
            var proveedor = await _repository.ObtenerPorIdAsync(id);

            if (proveedor is null)
                throw new ExcepcionReglaNegocio($"No existe un proveedor con ID: {id}", "PROVEEDOR_NO_ENCONTRADO");

            proveedor.Actualizar(dtoC.Nombre, dtoC.RNC, dtoC.Telefono, dtoC.CoberturaDefault);

            await _domainService.ValidarNombreUnicoAsync(dtoC.Nombre);

            await _repository.ActualizarAsync(proveedor);

            int usuarioidtemp = 0; // Temporal hasta que se implemente la autenticación
            await _auditoriaLogger.RegistrarAsync(usuarioidtemp, "Actualizar", "Proveedores", $"Proveedor actualizado con ID: {proveedor.Id}");

            return new ProveedoresResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };

        }


        public async Task<bool> EliminarAsync(int id)
        {
            var proveedor = await _repository.ObtenerPorIdAsync(id);

            if (proveedor is null)
                throw new ExcepcionReglaNegocio($"No existe un proveedor con ID: {id}", "PROVEEDOR_NO_ENCONTRADO");
        
            await _repository.EliminarAsync(id);

            int usuarioIdTemp = 0; // Temporal hasta que se implemente la autenticación
            await _auditoriaLogger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Proveedores", $"Proveedor eliminado con ID: {proveedor.Id}");
            return true;
        }

        public async Task<ProveedoresResponseDto> LeerAsync(int id)
        {
            var proveedor = await _repository.ObtenerPorIdAsync(id);

            if (proveedor is null)
                throw new ExcepcionReglaNegocio($"No existe un proveedor con ID: {id}", "PROVEEDOR_NO_ENCONTRADO");

            return new ProveedoresResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            };


        }

        public async Task<IReadOnlyList<ProveedoresResponseDto>> LeerTodosAsync()
        {
            var proveedores = await _repository.ObtenerTodosAsync();

            if (!proveedores.Any())
            {
                throw new ExcepcionReglaNegocio("No existen proveedores registrados", "PROVEEDORES_NO_ENCONTRADOS");
            }

            return proveedores.Select(proveedor => new ProveedoresResponseDto
            {
                Id = proveedor.Id,
                Nombre = proveedor.Nombre,
                RNC = proveedor.RNC,
                Telefono = proveedor.Telefono,
                CoberturaDefault = proveedor.CoberturaDefault
            }).ToList();
        }
    }
}
