namespace ControleEstoque.Models;

public sealed class Produto
{
    public int CodigoProduto { get; init; }
    public string DescricaoProduto { get; init; } = "";
    public int Estoque { get; set; }
}
