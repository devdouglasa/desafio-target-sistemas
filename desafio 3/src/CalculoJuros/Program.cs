using System.Globalization;
using CalculoJuros.Services;

var cultura = new CultureInfo("pt-BR");
CultureInfo.CurrentCulture = cultura;
CultureInfo.CurrentUICulture = cultura;

while (true)
{
    Console.WriteLine();
    Console.WriteLine("1 - Calcular juros");
    Console.WriteLine("0 - Sair");
    Console.Write("Opção: ");

    switch (Console.ReadLine()?.Trim())
    {
        case "1":
            Calcular();
            break;
        case "0":
        case null:
            return 0;
        default:
            Console.WriteLine("Opção inválida.");
            break;
    }
}

static void Calcular()
{
    Console.Write("Valor: ");
    if (!decimal.TryParse(Console.ReadLine(), out var valor) || valor <= 0m)
    {
        Console.WriteLine("Valor inválido.");
        return;
    }

    Console.Write("Vencimento (dd/MM/yyyy): ");
    if (!DateOnly.TryParseExact(
            Console.ReadLine()?.Trim(),
            "dd/MM/yyyy",
            CultureInfo.CurrentCulture,
            DateTimeStyles.None,
            out var vencimento))
    {
        Console.WriteLine("Data inválida.");
        return;
    }

    var hoje = DateOnly.FromDateTime(DateTime.Today);
    var resultado = CalculadoraJuros.Calcular(valor, vencimento, hoje);

    Console.WriteLine($"Dias de atraso: {resultado.DiasAtraso}");
    Console.WriteLine($"Juros: {resultado.Juros:C}");
    Console.WriteLine($"Valor atualizado: {resultado.ValorAtualizado:C}");
}
