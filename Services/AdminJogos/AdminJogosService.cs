using System.Net.Http.Headers;
using System.Net.Http.Json;
using Fcg.Web.DTOs.Catalogo;
using Microsoft.AspNetCore.Components.Forms;

namespace Fcg.Web.Services.AdminJogos;

public sealed class AdminJogosService : IAdminJogosService
{
    private const long TamanhoMaximoImagem = 5 * 1024 * 1024;

    private readonly HttpClient _httpClient;

    public AdminJogosService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<JogoResponseDto> CriarAsync(
        CriarJogoRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(dto);

        using var response = await _httpClient.PostAsJsonAsync(
            "api/v1/jogos",
            dto,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JogoResponseDto>(
                   cancellationToken)
               ?? throw new InvalidOperationException(
                   "A API não retornou o jogo criado.");
    }

    public async Task<JogoResponseDto> AtualizarAsync(
        string id,
        AtualizarJogoRequestDto dto,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);
        ArgumentNullException.ThrowIfNull(dto);

        using var response = await _httpClient.PutAsJsonAsync(
            $"api/v1/jogos/{Uri.EscapeDataString(id)}",
            dto,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        return await response.Content.ReadFromJsonAsync<JogoResponseDto>(
                   cancellationToken)
               ?? throw new InvalidOperationException(
                   "A API não retornou o jogo atualizado.");
    }

    public async Task DesativarAsync(
        string id,
        CancellationToken cancellationToken = default)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(id);

        using var response = await _httpClient.DeleteAsync(
            $"api/v1/jogos/{Uri.EscapeDataString(id)}",
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }

    public async Task<string> UploadImagemAsync(
        IBrowserFile arquivo,
        CancellationToken cancellationToken = default)
    {
        ArgumentNullException.ThrowIfNull(arquivo);

        if (arquivo.Size <= 0)
        {
            throw new InvalidOperationException(
                "Selecione um arquivo de imagem válido.");
        }

        if (arquivo.Size > TamanhoMaximoImagem)
        {
            throw new InvalidOperationException(
                "A imagem não pode ultrapassar 5 MB.");
        }

        var tipoConteudo = arquivo.ContentType.ToLowerInvariant();

        if (tipoConteudo is not "image/png"
            and not "image/jpeg"
            and not "image/webp")
        {
            throw new InvalidOperationException(
                "Formato inválido. Envie uma imagem PNG, JPEG ou WebP.");
        }

        using var conteudo = new MultipartFormDataContent();

        await using var stream = arquivo.OpenReadStream(
            TamanhoMaximoImagem,
            cancellationToken);

        using var conteudoArquivo = new StreamContent(stream);
        conteudoArquivo.Headers.ContentType =
            new MediaTypeHeaderValue(tipoConteudo);

        conteudo.Add(conteudoArquivo, "arquivo", arquivo.Name);

        using var response = await _httpClient.PostAsync(
            "api/v1/jogos/imagem",
            conteudo,
            cancellationToken);

        if (!response.IsSuccessStatusCode)
        {
            var mensagem = await response.Content.ReadAsStringAsync(
                cancellationToken);

            throw new HttpRequestException(
                $"Não foi possível enviar a imagem. " +
                $"Status: {(int)response.StatusCode}. {mensagem}");
        }

        var resultado =
            await response.Content.ReadFromJsonAsync<RespostaUploadImagem>(
                cancellationToken: cancellationToken);

        if (string.IsNullOrWhiteSpace(resultado?.ImagemUrl))
        {
            throw new InvalidOperationException(
                "A API não retornou a URL da imagem enviada.");
        }

        return resultado.ImagemUrl;
    }

    private sealed record RespostaUploadImagem(string ImagemUrl);
}
