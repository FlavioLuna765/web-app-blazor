using System.Net.Http.Json;
using Fcg.Web.DTOs.Usuario;
using Microsoft.AspNetCore.Components.Forms;

namespace Fcg.Web.Services.Perfil;

public sealed class PerfilService : IPerfilService
{
    private const long TamanhoMaximoFoto = 5 * 1024 * 1024;

    private readonly HttpClient _httpClient;

    public PerfilService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<UsuarioResponseDto> ObterAsync(
        string usuarioId,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(usuarioId);

        var usuario = await _httpClient.GetFromJsonAsync<UsuarioResponseDto>(
            $"api/v1/usuarios/{Uri.EscapeDataString(usuarioId)}",
            cancellationToken);

        return usuario
            ?? throw new InvalidOperationException(
                "A API não retornou os dados do usuário.");
    }

    public async Task<UsuarioResponseDto> AtualizarAsync(
        string usuarioId,
        AtualizarUsuarioRequestDto request,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(usuarioId);
        ArgumentNullException.ThrowIfNull(request);

        var response = await _httpClient.PutAsJsonAsync(
            $"api/v1/usuarios/{Uri.EscapeDataString(usuarioId)}",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var usuario = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>(
            cancellationToken);

        return usuario
            ?? throw new InvalidOperationException(
                "A API não retornou os dados atualizados do usuário.");
    }

    public async Task<UsuarioResponseDto> AtualizarFotoAsync(
        string usuarioId,
        IBrowserFile arquivo,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(usuarioId);
        ArgumentNullException.ThrowIfNull(arquivo);

        if (arquivo.Size > TamanhoMaximoFoto)
        {
            throw new InvalidOperationException(
                "A foto deve ter no máximo 5 MB.");
        }

        using var conteudo = new MultipartFormDataContent();

        await using var stream = arquivo.OpenReadStream(
            TamanhoMaximoFoto,
            cancellationToken);

        using var conteudoArquivo = new StreamContent(stream);

        if (!string.IsNullOrWhiteSpace(arquivo.ContentType))
        {
            conteudoArquivo.Headers.ContentType =
                new System.Net.Http.Headers.MediaTypeHeaderValue(
                    arquivo.ContentType);
        }

        conteudo.Add(
            conteudoArquivo,
            "arquivo",
            Path.GetFileName(arquivo.Name));

        var response = await _httpClient.PostAsync(
            $"api/v1/usuarios/{Uri.EscapeDataString(usuarioId)}/foto",
            conteudo,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var usuario = await response.Content.ReadFromJsonAsync<UsuarioResponseDto>(
            cancellationToken);

        return usuario
            ?? throw new InvalidOperationException(
                "A API não retornou os dados atualizados após o envio da foto.");
    }
}
