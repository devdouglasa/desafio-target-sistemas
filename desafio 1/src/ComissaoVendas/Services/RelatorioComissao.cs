using System.Globalization;
using ComissaoVendas.Models;

namespace ComissaoVendas.Services;

public static class RelatorioComissao
{
    private static readonly StringComparer ComparadorNome =
        StringComparer.Create(new CultureInfo("pt-BR"), ignoreCase: false);

    public static IReadOnlyList<ResumoVendedor> Gerar(IEnumerable<Venda> vendas)
    {
        return vendas
            .GroupBy(venda => venda.Vendedor)
            .Select(grupo => new ResumoVendedor(
                grupo.Key,
                grupo.Count(),
                grupo.Sum(venda => venda.Valor),
                grupo.Sum(venda => CalculadoraComissao.Calcular(venda.Valor))))
            .OrderBy(resumo => resumo.Vendedor, ComparadorNome)
            .ToList();
    }
}
