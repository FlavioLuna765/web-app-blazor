using System.Net.Http.Json;
using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Biblioteca;

public sealed class BibliotecaService : IBibliotecaService
{
    private readonly HttpClient _httpClient;

    public BibliotecaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<IReadOnlyList<JogoResponseDto>> ListarAsync(
        CancellationToken cancellationToken = default)
    {
        var jogos = await _httpClient.GetFromJsonAsync<List<JogoResponseDto>>(
            "api/v1/biblioteca",
            cancellationToken);

        return jogos ?? [];
    }
}
