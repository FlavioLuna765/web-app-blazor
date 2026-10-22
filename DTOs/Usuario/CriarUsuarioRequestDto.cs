namespace Fcg.Web.DTOs.Usuario;

public sealed record CriarUsuarioRequestDto(
    string Nome,
    string Email,
    string Senha);
