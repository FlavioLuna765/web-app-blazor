namespace Fcg.Web.DTOs.Catalogo;

public sealed record PaginacaoResponseDto<T>(
    IEnumerable<T> Itens,
    int Pagina,
    int TamanhoPagina,
    long Total);
