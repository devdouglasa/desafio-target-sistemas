using ComissaoVendas.Services;

namespace ComissaoVendas.Tests;

public class CalculadoraComissaoTests
{
    public static TheoryData<decimal> AbaixoDe100 => new()
    {
        0m,
        90.75m,
        99.99m
    };

    public static TheoryData<decimal, decimal> AbaixoDe500 => new()
    {
        { 250.30m, 2.50m },
        { 320.40m, 3.20m },
        { 480.75m, 4.81m },
        { 499.99m, 5.00m }
    };

    public static TheoryData<decimal, decimal> APartirDe500 => new()
    {
        { 1200.50m, 60.03m },
        { 1400.30m, 70.02m },
        { 1100.90m, 55.05m }
    };

    [Theory]
    [MemberData(nameof(AbaixoDe100))]
    public void AbaixoDe100_NaoGeraComissao(decimal valor)
    {
        Assert.Equal(0m, CalculadoraComissao.Calcular(valor));
    }

    [Fact]
    public void Exatamente100_GeraUmPorCento()
    {
        Assert.Equal(1.00m, CalculadoraComissao.Calcular(100m));
    }

    [Theory]
    [MemberData(nameof(AbaixoDe500))]
    public void AbaixoDe500_GeraUmPorCento(decimal valor, decimal esperado)
    {
        Assert.Equal(esperado, CalculadoraComissao.Calcular(valor));
    }

    [Fact]
    public void Exatamente500_GeraCincoPorCento()
    {
        Assert.Equal(25.00m, CalculadoraComissao.Calcular(500m));
    }

    [Theory]
    [MemberData(nameof(APartirDe500))]
    public void APartirDe500_GeraCincoPorCento(decimal valor, decimal esperado)
    {
        Assert.Equal(esperado, CalculadoraComissao.Calcular(valor));
    }
}
