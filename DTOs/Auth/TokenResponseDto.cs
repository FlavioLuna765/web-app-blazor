namespace Fcg.Web.DTOs.Auth;

public sealed record TokenResponseDto(
    string Token,
    DateTime Expiracao,
    int Tipo,
    string RefreshToken,
    DateTime RefreshTokenExpiracao);
