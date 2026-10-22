using Fcg.Web.DTOs.Catalogo;

namespace Fcg.Web.Services.Carrinho;

public sealed class CarrinhoService : ICarrinhoService
{
    private readonly List<JogoResponseDto> _itens = [];

    public IReadOnlyList<JogoResponseDto> Itens => _itens;

    public int Quantidade => _itens.Count;

    public decimal Total =>
        _itens.Sum(jogo => jogo.Preco);

    public void Adicionar(JogoResponseDto jogo)
    {
        ArgumentNullException.ThrowIfNull(jogo);

        if (Contem(jogo.Id))
            return;

        _itens.Add(jogo);
    }

    public void Remover(string jogoId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jogoId);

        var jogo = _itens.FirstOrDefault(
            item => item.Id == jogoId);

        if (jogo is not null)
        {
            _itens.Remove(jogo);
        }
    }

    public void Limpar()
    {
        _itens.Clear();
    }

    public bool Contem(string jogoId)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(jogoId);

        return _itens.Any(
            item => item.Id == jogoId);
    }
}
