using Fcg.Web.DTOs.Auth;

namespace Fcg.Web.Services.Auth;

public interface IAuthService
{
    Task<TokenResponseDto> LoginAsync(
        string email,
        string senha,
        CancellationToken cancellationToken = default);

    Task<TokenResponseDto> RefreshAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);

    Task LogoutAsync(
        string refreshToken,
        CancellationToken cancellationToken = default);
}
