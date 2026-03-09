using SGCM.Application.Base;
using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Application.Interfaces
{
    public interface IPacienteAppService : IAppService<CrearPacienteDto, PacienteResponseDto>
    {

    }
}
