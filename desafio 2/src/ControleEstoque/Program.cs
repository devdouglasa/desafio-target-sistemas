using System.Globalization;
using System.Text.Json;
using ControleEstoque.Models;
using ControleEstoque.Services;

var cultura = new CultureInfo("pt-BR");
CultureInfo.CurrentCulture = cultura;
CultureInfo.CurrentUICulture = cultura;

var caminho = args.Length > 0
    ? args[0]
    : Path.Combine(AppContext.BaseDirectory, "Data", "estoque.json");

if (!File.Exists(caminho))
{
    Console.Error.WriteLine($"Arquivo não encontrado: {caminho}");
    return 1;
}

var json = await File.ReadAllTextAsync(caminho);
var arquivo = JsonSerializer.Deserialize<ArquivoEstoque>(json, new JsonSerializerOptions
{
    PropertyNameCaseInsensitive = true
});

if (arquivo is null)
{
    Console.Error.WriteLine("Não foi possível ler o estoque.");
    return 1;
}

var movimentador = new MovimentadorEstoque(arquivo.Estoque);

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Listar produtos");
    Console.WriteLine("2 - Lançar movimentação");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            Listar(movimentador);
            break;
        case "2":
            Lancar(movimentador);
            break;
        case "0":
        case null:
            return 0;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

static void Listar(MovimentadorEstoque movimentador)
{
    Console.WriteLine($"{"Código",8} {"Produto",-32} {"Estoque",8}");
    foreach (var produto in movimentador.Produtos)
        Console.WriteLine($"{produto.CodigoProduto,8} {produto.DescricaoProduto,-32} {produto.Estoque,8}");
}

static void Lancar(MovimentadorEstoque movimentador)
{
    Console.Write("Código do produto: ");
    if (!int.TryParse(Console.ReadLine(), out var codigo))
    {
        Console.WriteLine("Código inválido.");
        return;
    }

    Console.Write("Tipo (E = entrada, S = saída): ");
    var tipo = Console.ReadLine()?.Trim().ToUpperInvariant() switch
    {
        "E" => TipoMovimentacao.Entrada,
        "S" => TipoMovimentacao.Saida,
        _ => (TipoMovimentacao?)null
    };

    if (tipo is null)
    {
        Console.WriteLine("Tipo inválido. Use E ou S.");
        return;
    }

    Console.Write("Quantidade: ");
    if (!int.TryParse(Console.ReadLine(), out var quantidade))
    {
        Console.WriteLine("Quantidade inválida.");
        return;
    }

    Console.Write("Descrição: ");
    var descricao = Console.ReadLine() ?? "";

    try
    {
        var resultado = movimentador.Lancar(codigo, tipo.Value, descricao, quantidade);
        Console.WriteLine($"Movimentação {resultado.Id} registrada: {resultado.Descricao}");
        Console.WriteLine(
            $"Estoque final de {resultado.DescricaoProduto} ({resultado.CodigoProduto}): {resultado.EstoqueFinal}");
    }
    catch (Exception ex) when (ex is ArgumentException or InvalidOperationException)
    {
        Console.WriteLine(ex.Message);
    }
}
