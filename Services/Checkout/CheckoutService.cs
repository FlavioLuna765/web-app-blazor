using System.Net.Http.Json;
using Fcg.Web.DTOs.Checkout;
using Fcg.Web.Services.Usuario;

namespace Fcg.Web.Services.Checkout;

public sealed class CheckoutService : ICheckoutService
{
    private readonly HttpClient _httpClient;
    private readonly HttpClient _paymentsClient;
    private readonly IUsuarioAtualService _usuarioAtualService;

    public CheckoutService(
        HttpClient httpClient,
        HttpClient paymentsClient,
        IUsuarioAtualService usuarioAtualService)
    {
        _httpClient = httpClient;
        _paymentsClient = paymentsClient;
        _usuarioAtualService = usuarioAtualService;
    }

    public async Task<CompraAceitaResponseDto> AdquirirAsync(string jogoId)
    {
        var request = new AdquirirJogoRequestDto(jogoId);

        using var response = await _httpClient.PostAsJsonAsync(
            "api/v1/biblioteca",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var mensagem = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                mensagem = $"A API retornou HTTP {(int)response.StatusCode} ({response.StatusCode}).";
            }

            throw new InvalidOperationException(mensagem);
        }

        var resultado = await response.Content
            .ReadFromJsonAsync<CompraAceitaResponseDto>();

        return resultado
            ?? throw new InvalidOperationException(
                "A API não retornou os dados da compra.");
    }

    public async Task<PedidoResponseDto> ObterPedidoAsync(string orderId)
    {
        if (string.IsNullOrWhiteSpace(orderId))
        {
            throw new InvalidOperationException(
                "O identificador do pedido não foi informado.");
        }

        using var response = await _httpClient.GetAsync(
            $"api/v1/pedidos/{Uri.EscapeDataString(orderId)}");

        if (!response.IsSuccessStatusCode)
        {
            var mensagem = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                mensagem = $"Não foi possível consultar o pedido. HTTP {(int)response.StatusCode} ({response.StatusCode}).";
            }

            throw new InvalidOperationException(mensagem);
        }

        return await response.Content.ReadFromJsonAsync<PedidoResponseDto>()
            ?? throw new InvalidOperationException(
                "A API não retornou os dados do pedido.");
    }

    public async Task<string> CriarCheckoutMercadoPagoAsync(
        string orderId,
        string gameId,
        decimal price)
    {
        // O pedido é consultado na API de catálogo usando o cliente autenticado.
        var pedido = await ObterPedidoAsync(orderId);

        // O usuário da sessão deve corresponder ao proprietário do pedido.
        var usuarioId = _usuarioAtualService.ObterId();

        if (string.IsNullOrWhiteSpace(usuarioId))
        {
            throw new InvalidOperationException(
                "Sua sessão não possui um usuário autenticado válido. Entre novamente.");
        }

        if (!string.Equals(
                pedido.UserId,
                usuarioId,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "O pedido consultado não pertence ao usuário autenticado.");
        }

        if (!string.Equals(
                pedido.GameId,
                gameId,
                StringComparison.Ordinal))
        {
            throw new InvalidOperationException(
                "O jogo informado não corresponde ao jogo registrado no pedido.");
        }

        if (pedido.Preco <= 0)
        {
            throw new InvalidOperationException(
                "O pedido possui um preço inválido.");
        }

        // O backend de catálogo é a fonte dos dados usados no pagamento.
        var request = new CheckoutMercadoPagoRequest(
            pedido.OrderId,
            pedido.UserId,
            pedido.GameId,
            pedido.Preco);

        using var response = await _paymentsClient.PostAsJsonAsync(
            "api/v1/payments/checkout",
            request);

        if (!response.IsSuccessStatusCode)
        {
            var mensagem = await response.Content.ReadAsStringAsync();

            if (string.IsNullOrWhiteSpace(mensagem))
            {
                mensagem = $"A API de pagamentos retornou HTTP {(int)response.StatusCode} ({response.StatusCode}).";
            }

            throw new InvalidOperationException(
                $"Não foi possível criar o checkout no Mercado Pago: {mensagem}");
        }

        var resultado = await response.Content
            .ReadFromJsonAsync<CheckoutMercadoPagoResponse>();

        if (string.IsNullOrWhiteSpace(resultado?.CheckoutUrl))
        {
            throw new InvalidOperationException(
                "A API de pagamentos não retornou a URL do checkout.");
        }

        return resultado.CheckoutUrl;
    }
}

public sealed record CheckoutMercadoPagoRequest(
    string OrderId,
    string UserId,
    string GameId,
    decimal Price);

public sealed record CheckoutMercadoPagoResponse(
    string CheckoutUrl);
