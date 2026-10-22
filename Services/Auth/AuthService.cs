using System.Net.Http.Json;
using Fcg.Web.DTOs.Auth;

namespace Fcg.Web.Services.Auth;

public sealed class AuthService : IAuthService
{
    private readonly HttpClient _httpClient;

    public AuthService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<TokenResponseDto> LoginAsync(
        string email,
        string senha,
        CancellationToken cancellationToken = default)
    {
        var request = new LoginRequestDto(email, senha);

        var response = await _httpClient.PostAsJsonAsync(
            "api/v1/auth/login",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>(
            cancellationToken);

        return result
            ?? throw new InvalidOperationException(
                "A API não retornou os dados de autenticação.");
    }

    public async Task<TokenResponseDto> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var request = new RefreshTokenRequestDto(refreshToken);

        var response = await _httpClient.PostAsJsonAsync(
            "api/v1/auth/refresh",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();

        var result = await response.Content.ReadFromJsonAsync<TokenResponseDto>(
            cancellationToken);

        return result
            ?? throw new InvalidOperationException(
                "A API não retornou os dados de autenticação.");
    }

    public async Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default)
    {
        var request = new RefreshTokenRequestDto(refreshToken);

        var response = await _httpClient.PostAsJsonAsync(
            "api/v1/auth/logout",
            request,
            cancellationToken);

        response.EnsureSuccessStatusCode();
    }
}
