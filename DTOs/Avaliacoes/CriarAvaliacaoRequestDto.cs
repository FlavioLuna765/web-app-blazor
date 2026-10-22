namespace Fcg.Web.DTOs.Avaliacoes;

public sealed record CriarAvaliacaoRequestDto(
    string JogoId,
    int Nota,
    string Comentario,
    string? Titulo = null,
    IReadOnlyList<string>? Tags = null,
    IReadOnlyDictionary<string, string>? Contexto = null);
