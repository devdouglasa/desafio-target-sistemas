using CalculoJuros.Services;

namespace CalculoJuros.Tests;

public class CalculadoraJurosTests
{
    private static readonly DateOnly Hoje = new(2026, 10, 4);

    [Fact]
    public void UmDiaDeAtraso_CobraDoisEMeioPorCento()
    {
        var resultado = CalculadoraJuros.Calcular(100m, Hoje.AddDays(-1), Hoje);

        Assert.Equal(1, resultado.DiasAtraso);
        Assert.Equal(2.50m, resultado.Juros);
        Assert.Equal(102.50m, resultado.ValorAtualizado);
    }

    [Fact]
    public void DezDiasDeAtraso_CobraJurosSimples()
    {
        var resultado = CalculadoraJuros.Calcular(1000m, Hoje.AddDays(-10), Hoje);

        Assert.Equal(10, resultado.DiasAtraso);
        Assert.Equal(250.00m, resultado.Juros);
        Assert.Equal(1250.00m, resultado.ValorAtualizado);
    }

    [Fact]
    public void VencimentoHoje_NaoGeraJuros()
    {
        var resultado = CalculadoraJuros.Calcular(1000m, Hoje, Hoje);

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.Juros);
        Assert.Equal(1000m, resultado.ValorAtualizado);
    }

    [Fact]
    public void VencimentoFuturo_NaoGeraJuros()
    {
        var resultado = CalculadoraJuros.Calcular(1000m, Hoje.AddDays(5), Hoje);

        Assert.Equal(0, resultado.DiasAtraso);
        Assert.Equal(0m, resultado.Juros);
    }

    [Fact]
    public void ArredondaJurosParaDuasCasas()
    {
        var resultado = CalculadoraJuros.Calcular(200.50m, Hoje.AddDays(-3), Hoje);

        Assert.Equal(15.04m, resultado.Juros);
        Assert.Equal(215.54m, resultado.ValorAtualizado);
    }

    [Theory]
    [InlineData(0)]
    [InlineData(-10)]
    public void ValorInvalido_LancaExcecao(int valor)
    {
        Assert.Throws<ArgumentException>(() =>
            CalculadoraJuros.Calcular(valor, Hoje, Hoje));
    }
}
