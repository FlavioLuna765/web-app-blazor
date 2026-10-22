using Fcg.Web.DTOs.Checkout;

namespace Fcg.Web.Services.Checkout;

public interface ICheckoutService
{
    Task<CompraAceitaResponseDto> AdquirirAsync(string jogoId);

    Task<PedidoResponseDto> ObterPedidoAsync(string orderId);

    Task<string> CriarCheckoutMercadoPagoAsync(
    string orderId,
    string gameId,
    decimal price);
}
