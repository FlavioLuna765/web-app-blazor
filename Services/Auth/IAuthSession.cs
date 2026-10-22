using Fcg.Web.DTOs.Auth;

namespace Fcg.Web.Services.Auth;

public interface IAuthSession
{
    bool IsAuthenticated { get; }

    string? Token { get; }

    string? RefreshToken { get; }

    DateTime? Expiracao { get; }

    DateTime? RefreshTokenExpiracao { get; }

    int? Tipo { get; }

    event Action? SessionChanged;

    void SetSession(TokenResponseDto tokenResponse);

    void Clear();
}
