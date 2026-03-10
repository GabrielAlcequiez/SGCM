using SGCM.Application.Base;
using SGCM.Application.Dtos.Pacientes;

namespace SGCM.Application.Interfaces.Pacientes
{
    public interface IPacienteAppService : IAppService<CrearPacienteDto, PacienteResponseDto>
    {

    }
}
