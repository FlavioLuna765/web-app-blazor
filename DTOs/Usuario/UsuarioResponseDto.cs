namespace Fcg.Web.DTOs.Usuario;

public sealed record UsuarioResponseDto(
    string Id,
    string Nome,
    string Email,
    int Tipo,
    DateTime DataCriacao,
    bool Ativo,
    string? FotoUrl = null);
