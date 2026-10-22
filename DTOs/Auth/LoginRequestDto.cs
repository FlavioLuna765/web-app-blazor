namespace Fcg.Web.DTOs.Auth;

public sealed record LoginRequestDto(
    string Email,
    string Senha);
