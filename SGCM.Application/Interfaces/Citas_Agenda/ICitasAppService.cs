using SGCM.Application.Base;
using SGCM.Application.Dtos.Citas_Agenda;

namespace SGCM.Application.Interfaces.Citas_Agenda
{
    public interface ICitasAppService : IAppService<CrearCitaDto, CitaResponseDto>
    {
        Task<bool> CancelarCitaAsync(int id);
    }
}
