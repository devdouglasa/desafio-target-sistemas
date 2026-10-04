using ControleEstoque.Models;

namespace ControleEstoque.Services;

public sealed class MovimentadorEstoque
{
    private readonly Dictionary<int, Produto> _produtos;
    private readonly List<Movimentacao> _movimentacoes = [];
    private int _proximoId = 1;

    public MovimentadorEstoque(IEnumerable<Produto> produtos)
    {
        _produtos = produtos.ToDictionary(produto => produto.CodigoProduto);
    }

    public IReadOnlyList<Produto> Produtos =>
        _produtos.Values.OrderBy(produto => produto.CodigoProduto).ToList();

    public IReadOnlyList<Movimentacao> Movimentacoes => _movimentacoes;

    public ResultadoMovimentacao Lancar(
        int codigoProduto,
        TipoMovimentacao tipo,
        string descricao,
        int quantidade)
    {
        if (string.IsNullOrWhiteSpace(descricao))
            throw new ArgumentException("A descrição da movimentação é obrigatória.", nameof(descricao));

        if (quantidade <= 0)
            throw new ArgumentException("A quantidade deve ser maior que zero.", nameof(quantidade));

        if (!_produtos.TryGetValue(codigoProduto, out var produto))
            throw new InvalidOperationException($"Produto {codigoProduto} não encontrado.");

        var estoqueFinal = tipo switch
        {
            TipoMovimentacao.Entrada => produto.Estoque + quantidade,
            TipoMovimentacao.Saida => produto.Estoque - quantidade,
            _ => throw new ArgumentOutOfRangeException(nameof(tipo), "Tipo de movimentação inválido.")
        };

        if (estoqueFinal < 0)
        {
            throw new InvalidOperationException(
                $"Saída de {quantidade} excede o estoque de {produto.DescricaoProduto} ({produto.Estoque}).");
        }

        var movimentacao = new Movimentacao(
            _proximoId,
            codigoProduto,
            tipo,
            descricao.Trim(),
            quantidade);

        produto.Estoque = estoqueFinal;
        _movimentacoes.Add(movimentacao);
        _proximoId++;

        return new ResultadoMovimentacao(
            movimentacao.Id,
            produto.CodigoProduto,
            produto.DescricaoProduto,
            movimentacao.Descricao,
            estoqueFinal);
    }
}
