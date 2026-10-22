using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Carrinho;

public interface ICarrinhoService
{
    IReadOnlyList<JogoResponseDto> Itens { get; }

    int Quantidade { get; }

    decimal Total { get; }

    void Adicionar(JogoResponseDto jogo);

    void Remover(string jogoId);

    void Limpar();

    bool Contem(string jogoId);
}
