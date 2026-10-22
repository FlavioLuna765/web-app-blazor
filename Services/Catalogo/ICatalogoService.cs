using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Catalogo;

public interface ICatalogoService
{
    Task<PaginacaoResponseDto<JogoResponseDto>> ObterJogosAsync(
        int pagina = 1,
        int tamanhoPagina = 10,
        int? genero = null,
        CancellationToken cancellationToken = default);

    Task<JogoResponseDto> ObterJogoAsync(
        string id,
        CancellationToken cancellationToken = default);
}
