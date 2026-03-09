namespace SGCM.Application.Base
{
    public interface IAppService<TCrearDto, TResponseDto>
    {
        Task<TResponseDto> CrearAsync(TCrearDto dto);

        Task<TResponseDto> LeerAsync(int id);

        Task<IReadOnlyList<TResponseDto>> LeerTodosAsync();

        Task<TResponseDto> ActualizarAsync(int id, TCrearDto dtoC);

        Task<bool> EliminarAsync(int id);

    }
}
