using System.Globalization;
using System.Text.Json;
using ComissaoVendas.Models;
using ComissaoVendas.Services;

var cultura = new CultureInfo("pt-BR");
CultureInfo.CurrentCulture = cultura;
CultureInfo.CurrentUICulture = cultura;

var caminho = args.Length > 0
    ? args[0]
    : Path.Combine(AppContext.BaseDirectory, "Data", "vendas.json");

if (!File.Exists(caminho))
{
    Console.Error.WriteLine($"Arquivo não encontrado: {caminho}");
    return 1;
}

var json = await File.ReadAllTextAsync(caminho);
var arquivo = JsonSerializer.Deserialize<ArquivoVendas>(json, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

if (arquivo is null)
{
    Console.Error.WriteLine("Não foi possível ler as vendas.");
    return 1;
}

var resumo = RelatorioComissao.Gerar(arquivo.Vendas);

Console.WriteLine($"{"Vendedor",-20} {"Vendas",8} {"Total vendido",16} {"Comissão",14}");
foreach (var item in resumo)
{
    Console.WriteLine(
        $"{item.Vendedor,-20} {item.QuantidadeVendas,8} {item.TotalVendido,16:C} {item.TotalComissao,14:C}");
}

return 0;
