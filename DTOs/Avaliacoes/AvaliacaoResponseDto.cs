namespace Fcg.Web.DTOs.Avaliacoes;

public sealed record AvaliacaoResponseDto(
    string Id,
    string JogoId,
    string UsuarioId,
    string? Titulo,
    string Comentario,
    IReadOnlyList<string> Tags,
    IReadOnlyDictionary<string, string> Contexto,
    int VotosUteis,
    DateTime DataCriacao);
