namespace Fcg.Web.DTOs.Checkout;

public sealed record PedidoResponseDto(
    string OrderId,
    string UserId,
    string GameId,
    decimal Preco,
    string Status,
    DateTime DataCriacao,
    DateTime DataAtualizacao);
