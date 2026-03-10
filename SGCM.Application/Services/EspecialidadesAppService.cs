using SGCM.Application.Dtos.Citas_Agenda;
using SGCM.Application.Interfaces;
using SGCM.Application.Logger;
using SGCM.Domain.Entities.Medicos;
using SGCM.Domain.Exceptions;
using SGCM.Domain.Repository.Citas_Agenda;
using SGCM.Domain.Services.Interfaces.ICitas;

namespace SGCM.Application.Services
{
    public class EspecialidadesAppService : IEspecialidadesAppService
    {
        private readonly IEspecialidadesRepository _repository;
        private readonly IEspecialidadDomainService _domainService;
        private readonly IAuditoriaLogger _logger;
        public EspecialidadesAppService(IEspecialidadesRepository repository, IEspecialidadDomainService domainService, IAuditoriaLogger logger)
        {
            _domainService = domainService;
            _repository = repository;
            _logger = logger;
        }

        public async Task<EspecialidadesResponseDto> CrearAsync(CrearEspecialidadesDto dto)
        {
            await _domainService.EsNombreUnicoAsync(dto.Nombre);

            var especialidad = new Especialidades
            (
                dto.Nombre,
                dto.Descripcion
            );
            
            await _repository.AgregarAsync(especialidad);
            int usuarioidTemp = 0;
            await _logger.RegistrarAsync(usuarioidTemp, "Crear", "Especialidades", $"Se creó la especialidad {especialidad.Nombre} con ID: {especialidad.Id}");

            return new EspecialidadesResponseDto
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };



        }

        public async Task<EspecialidadesResponseDto> ActualizarAsync(int id, CrearEspecialidadesDto dtoC)
        {
            var especialidad = await _repository.ObtenerPorIdAsync(id);
            if (especialidad is null)
                throw new ExcepcionNoEncontrado("Especialidades", id);

            if (especialidad.Nombre != dtoC.Nombre)
            {
                await _domainService.EsNombreUnicoAsync(dtoC.Nombre);
            }

            especialidad.Actualizar(dtoC.Nombre, dtoC.Descripcion);

            await _repository.ActualizarAsync(especialidad);
            int usuarioIdTemp = 0;
            await _logger.RegistrarAsync(usuarioIdTemp, "Actualizar", "Especialidades", $"Se actualizó la especialidad {especialidad.Nombre} con ID: {especialidad.Id}");
        
            return new EspecialidadesResponseDto
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };
        }


        public async Task<bool> EliminarAsync(int id)
        {
            var especialidad = await _repository.ObtenerPorIdAsync(id);

            if (especialidad is null)
                throw new ExcepcionNoEncontrado("Especialidades", id);

            await _domainService.PuedeEliminarEspecialidadAsync(id);

            await _repository.EliminarAsync(id);
            int usuarioIdTemp = 0;
            await _logger.RegistrarAsync(usuarioIdTemp, "Eliminar", "Especialidades", $"Se eliminó la especialidad {especialidad.Nombre} con ID: {especialidad.Id}");
            return true;
        }

        public async Task<EspecialidadesResponseDto> LeerAsync(int id)
        {
            var especialidad = await _repository.ObtenerPorIdAsync(id);
            if (especialidad is null)
                throw new ExcepcionNoEncontrado("Especialidades", id);

            return new EspecialidadesResponseDto
            {
                Id = especialidad.Id,
                Nombre = especialidad.Nombre,
                Descripcion = especialidad.Descripcion
            };

        }

        public async Task<IReadOnlyList<EspecialidadesResponseDto>> LeerTodosAsync()
        {
            var lista = await _repository.ObtenerTodosAsync();
            return lista.Select(e => new EspecialidadesResponseDto
            {
                Id = e.Id,
                Nombre = e.Nombre,
                Descripcion = e.Descripcion
            }).ToList();

        }
    }
}
