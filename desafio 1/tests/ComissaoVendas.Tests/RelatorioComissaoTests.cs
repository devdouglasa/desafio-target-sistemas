using System.Text.Json;
using ComissaoVendas.Models;
using ComissaoVendas.Services;

namespace ComissaoVendas.Tests;

public class RelatorioComissaoTests
{
    [Fact]
    public void AgrupaPorVendedor_SomaVendasEComissao()
    {
        var vendas = new[]
        {
            new Venda("Maria Souza", 90.75m),
            new Venda("João Silva", 250.30m),
            new Venda("João Silva", 1200.50m),
            new Venda("Maria Souza", 500m)
        };

        var resumo = RelatorioComissao.Gerar(vendas);

        Assert.Equal(["João Silva", "Maria Souza"], resumo.Select(item => item.Vendedor));

        var joao = resumo[0];
        Assert.Equal(2, joao.QuantidadeVendas);
        Assert.Equal(1450.80m, joao.TotalVendido);
        Assert.Equal(62.53m, joao.TotalComissao);

        var maria = resumo[1];
        Assert.Equal(2, maria.QuantidadeVendas);
        Assert.Equal(590.75m, maria.TotalVendido);
        Assert.Equal(25.00m, maria.TotalComissao);
    }

    [Fact]
    public void ArquivoDeVendas_TotalizaComissaoDeCadaVendedor()
    {
        var caminho = Path.Combine(AppContext.BaseDirectory, "Data", "vendas.json");
        var json = File.ReadAllText(caminho);
        var arquivo = JsonSerializer.Deserialize<ArquivoVendas>(json, new JsonSerializerOptions
        {
            PropertyNameCaseInsensitive = true
        });

        Assert.NotNull(arquivo);

        var comissoes = RelatorioComissao.Gerar(arquivo.Vendas)
            .ToDictionary(item => item.Vendedor, item => item.TotalComissao);

        Assert.Equal(495.69m, comissoes["João Silva"]);
        Assert.Equal(465.96m, comissoes["Maria Souza"]);
        Assert.Equal(379.38m, comissoes["Carlos Oliveira"]);
        Assert.Equal(404.99m, comissoes["Ana Lima"]);
    }
}
