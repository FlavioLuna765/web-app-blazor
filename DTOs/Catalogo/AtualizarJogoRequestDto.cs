namespace Fcg.Web.DTOs.Catalogo;

public sealed record AtualizarJogoRequestDto(
    string Titulo,
    string Descricao,
    int Genero,
    decimal Preco,
    string? ImagemUrl = null);
