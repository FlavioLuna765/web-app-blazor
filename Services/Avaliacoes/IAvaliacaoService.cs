using Fcg.Web.DTOs.Avaliacoes;
using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Avaliacoes;

public interface IAvaliacaoService
{
    Task<PaginacaoResponseDto<AvaliacaoResponseDto>> ListarPorJogoAsync(
        string jogoId,
        int pagina = 1,
        int tamanhoPagina = 10,
        CancellationToken cancellationToken = default);

    Task<AvaliacaoResumoResponseDto> ObterResumoAsync(
        string jogoId,
        CancellationToken cancellationToken = default);

    Task<AvaliacaoResponseDto> CriarAsync(
        CriarAvaliacaoRequestDto request,
        CancellationToken cancellationToken = default);

    Task<int> VotarUtilAsync(
        string avaliacaoId,
        CancellationToken cancellationToken = default);

    Task RemoverAsync(
        string avaliacaoId,
        CancellationToken cancellationToken = default);
}
