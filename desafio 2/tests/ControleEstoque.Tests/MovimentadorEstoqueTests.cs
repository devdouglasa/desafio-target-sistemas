using ControleEstoque.Models;
using ControleEstoque.Services;

namespace ControleEstoque.Tests;

public class MovimentadorEstoqueTests
{
    private static MovimentadorEstoque Criar()
    {
        return new MovimentadorEstoque(
        [
            new Produto { CodigoProduto = 101, DescricaoProduto = "Caneta Azul", Estoque = 150 },
            new Produto { CodigoProduto = 102, DescricaoProduto = "Caderno Universitário", Estoque = 75 }
        ]);
    }

    [Fact]
    public void Entrada_SomaEstoqueERetornaQuantidadeFinal()
    {
        var movimentador = Criar();

        var resultado = movimentador.Lancar(101, TipoMovimentacao.Entrada, "Compra do fornecedor", 10);

        Assert.Equal(1, resultado.Id);
        Assert.Equal("Compra do fornecedor", resultado.Descricao);
        Assert.Equal(160, resultado.EstoqueFinal);
        Assert.Equal(160, movimentador.Produtos.Single(produto => produto.CodigoProduto == 101).Estoque);
    }

    [Fact]
    public void Saida_SubtraiEstoque()
    {
        var movimentador = Criar();

        var resultado = movimentador.Lancar(102, TipoMovimentacao.Saida, "Venda no balcão", 25);

        Assert.Equal(50, resultado.EstoqueFinal);
    }

    [Fact]
    public void Saida_PodeZerarEstoque()
    {
        var movimentador = Criar();

        var resultado = movimentador.Lancar(102, TipoMovimentacao.Saida, "Ajuste de inventário", 75);

        Assert.Equal(0, resultado.EstoqueFinal);
    }

    [Fact]
    public void Ids_SaoSequenciaisEUnicos()
    {
        var movimentador = Criar();

        var primeira = movimentador.Lancar(101, TipoMovimentacao.Entrada, "Compra", 1);
        var segunda = movimentador.Lancar(101, TipoMovimentacao.Saida, "Venda", 1);

        Assert.Equal(1, primeira.Id);
        Assert.Equal(2, segunda.Id);
        Assert.Equal([1, 2], movimentador.Movimentacoes.Select(movimentacao => movimentacao.Id));
    }

    [Fact]
    public void ProdutoInexistente_LancaExcecao()
    {
        var movimentador = Criar();

        var excecao = Assert.Throws<InvalidOperationException>(() =>
            movimentador.Lancar(999, TipoMovimentacao.Entrada, "Compra", 1));

        Assert.Contains("999", excecao.Message);
        Assert.Empty(movimentador.Movimentacoes);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-1)]
    public void QuantidadeInvalida_LancaExcecao(int quantidade)
    {
        var movimentador = Criar();

        Assert.Throws<ArgumentException>(() =>
            movimentador.Lancar(101, TipoMovimentacao.Entrada, "Compra", quantidade));
    }

    [Theory]
    [InlineData("")]
    [InlineData("   ")]
    public void DescricaoVazia_LancaExcecao(string descricao)
    {
        var movimentador = Criar();

        Assert.Throws<ArgumentException>(() =>
            movimentador.Lancar(101, TipoMovimentacao.Entrada, descricao, 1));
    }

    [Fact]
    public void SaidaAcimaDoSaldo_NaoAlteraEstoque()
    {
        var movimentador = Criar();

        Assert.Throws<InvalidOperationException>(() =>
            movimentador.Lancar(101, TipoMovimentacao.Saida, "Venda", 151));

        Assert.Equal(150, movimentador.Produtos.Single(produto => produto.CodigoProduto == 101).Estoque);
        Assert.Empty(movimentador.Movimentacoes);
    }
}
