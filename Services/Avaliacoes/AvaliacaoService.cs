using System.Net.Http.Json;
using Fcg.Web.DTOs.Avaliacoes;
using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Avaliacoes;

public sealed class AvaliacaoService : IAvaliacaoService
{
    private readonly HttpClient _httpClient;

    public AvaliacaoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginacaoResponseDto<AvaliacaoResponseDto>>
        ListarPorJogoAsync(
            string jogoId,
            int pagina = 1,
            int tamanhoPagina = 10,
            CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jogoId);

        var url =
            $"api/v1/jogos/{Uri.EscapeDataString(jogoId)}/avaliacoes" +
            $"?pagina={pagina}&tamanhoPagina={tamanhoPagina}";

        var resultado = await _httpClient.GetFromJsonAsync<
            PaginacaoResponseDto<AvaliacaoResponseDto>>(
                url,
                cancellationToken);

        return resultado
            ?? throw new InvalidOperationException(
                "A API não retornou a lista de avaliações.");
    }

    public async Task<AvaliacaoResumoResponseDto> ObterResumoAsync(
        string jogoId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jogoId);

        var url =
            $"api/v1/jogos/{Uri.EscapeDataString(jogoId)}/avaliacoes/resumo";

        var resultado = await _httpClient.GetFromJsonAsync<
            AvaliacaoResumoResponseDto>(
                url,
                cancellationToken);

        return resultado
            ?? throw new InvalidOperationException(
                "A API não retornou o resumo das avaliações.");
    }

    public async Task<AvaliacaoResponseDto> CriarAsync(
        CriarAvaliacaoRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(request);

        using var resposta = await _httpClient.PostAsJsonAsync(
            "api/v1/avaliacoes",
            request,
            cancellationToken);

        resposta.EnsureSuccessStatusCode();

        var resultado = await resposta.Content
            .ReadFromJsonAsync<AvaliacaoResponseDto>(
                cancellationToken: cancellationToken);

        return resultado
            ?? throw new InvalidOperationException(
                "A API não retornou a avaliação criada.");
    }

    public async Task<int> VotarUtilAsync(
        string avaliacaoId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(avaliacaoId);

        using var resposta = await _httpClient.PostAsync(
            $"api/v1/avaliacoes/{Uri.EscapeDataString(avaliacaoId)}/util",
            content: null,
            cancellationToken);

        resposta.EnsureSuccessStatusCode();

        return await resposta.Content.ReadFromJsonAsync<int>(
            cancellationToken: cancellationToken);
    }

    public async Task RemoverAsync(
        string avaliacaoId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(avaliacaoId);

        using var resposta = await _httpClient.DeleteAsync(
            $"api/v1/avaliacoes/{Uri.EscapeDataString(avaliacaoId)}",
            cancellationToken);

        resposta.EnsureSuccessStatusCode();
    }
}
