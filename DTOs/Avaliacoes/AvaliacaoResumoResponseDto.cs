namespace Fcg.Web.DTOs.Avaliacoes;

public sealed record AvaliacaoResumoResponseDto(
    string JogoId,
    long Total,
    decimal MediaNota,
    IReadOnlyDictionary<int, long> Distribuicao);
