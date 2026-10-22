namespace Fcg.Web.DTOs.Catalogo;

public sealed record CriarJogoRequestDto(
    string Titulo,
    string Descricao,
    int Genero,
    decimal Preco,
    DateTime DataLancamento,
    string? ImagemUrl = null);
