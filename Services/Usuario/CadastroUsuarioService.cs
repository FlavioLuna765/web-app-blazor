using System.Net.Http.Json;
using Fcg.Web.DTOs.Usuario;

namespace Fcg.Web.Services.Usuario;

public sealed class CadastroUsuarioService : ICadastroUsuarioService
{
    private readonly HttpClient _httpClient;

    public CadastroUsuarioService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task CadastrarAsync(CriarUsuarioRequestDto request)
    {
        using var response = await _httpClient.PostAsJsonAsync(
            "api/v1/usuarios",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var mensagem = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                mensagem =
                    $"A API retornou o status HTTP {(int)response.StatusCode} ({response.StatusCode}).";
            }

            throw new InvalidOperationException(mensagem);
        }
    }
}
