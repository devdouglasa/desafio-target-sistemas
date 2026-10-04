namespace ControleEstoque.Models;

public sealed record Movimentacao(
    int Id,
    int CodigoProduto,
    TipoMovimentacao Tipo,
    string Descricao,
    int Quantidade);
