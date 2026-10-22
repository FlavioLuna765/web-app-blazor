using System.Net.Http.Json;
using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Catalogo;

public sealed class CatalogoService : ICatalogoService
{
    private readonly HttpClient _httpClient;

    public CatalogoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<PaginacaoResponseDto<JogoResponseDto>> ObterJogosAsync(
        int pagina = 1,
        int tamanhoPagina = 10,
        int? genero = null,
        CancellationToken cancellationToken = default)
    {
        var url =
            $"api/v1/jogos?pagina={pagina}&tamanhoPagina={tamanhoPagina}";

        if (genero.HasValue)
        {
            url += $"&genero={genero.Value}";
        }

        var resultado = await _httpClient.GetFromJsonAsync<
            PaginacaoResponseDto<JogoResponseDto>>(
            url,
            cancellationToken);

        return resultado
            ?? throw new InvalidOperationException(
                "A API de catálogo não retornou os jogos.");
    }

    public async Task<JogoResponseDto> ObterJogoAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        var resultado = await _httpClient.GetFromJsonAsync<JogoResponseDto>(
            $"api/v1/jogos/{id}",
            cancellationToken);

        return resultado
            ?? throw new InvalidOperationException(
                "A API de catálogo não retornou o jogo.");
    }
}
