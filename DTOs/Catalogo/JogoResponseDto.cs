namespace Fcg.Web.DTOs.Catalogo;

public sealed record JogoResponseDto(
    string Id,
    string Titulo,
    string Descricao,
    int Genero,
    decimal Preco,
    string Moeda,
    DateTime DataLancamento,
    bool Ativo,
    string? ImagemUrl = null);
