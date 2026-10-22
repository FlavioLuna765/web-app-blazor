using Fcg.Web.DTOs.Auth;

namespace Fcg.Web.Services.Auth;

public sealed class AuthSession : IAuthSession
{
    public bool IsAuthenticated =>
        !string.IsNullOrWhiteSpace(Token);

    public string? Token { get; private set; }

    public string? RefreshToken { get; private set; }

    public DateTime? Expiracao { get; private set; }

    public DateTime? RefreshTokenExpiracao { get; private set; }

    public int? Tipo { get; private set; }

    public event Action? SessionChanged;

    public void SetSession(TokenResponseDto tokenResponse)
    {
        ArgumentNullException.ThrowIfNull(tokenResponse);

        Token = tokenResponse.Token;
        RefreshToken = tokenResponse.RefreshToken;
        Expiracao = tokenResponse.Expiracao;
        RefreshTokenExpiracao = tokenResponse.RefreshTokenExpiracao;
        Tipo = tokenResponse.Tipo;

        SessionChanged?.Invoke();
    }

    public void Clear()
    {
        Token = null;
        RefreshToken = null;
        Expiracao = null;
        RefreshTokenExpiracao = null;
        Tipo = null;

        SessionChanged?.Invoke();
    }
}
