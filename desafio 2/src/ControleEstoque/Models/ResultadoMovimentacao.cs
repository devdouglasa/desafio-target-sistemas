namespace ControleEstoque.Models;

public sealed record ResultadoMovimentacao(
    int Id,
    int CodigoProduto,
    string DescricaoProduto,
    string Descricao,
    int EstoqueFinal);
