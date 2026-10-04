using CalculoJuros.Models;

namespace CalculoJuros.Services;

public static class CalculadoraJuros
{
    private const decimal TaxaDiaria = 0.025m;

    public static ResultadoJuros Calcular(decimal valor, DateOnly vencimento, DateOnly hoje)
    {
        if (valor <= 0m)
            throw new ArgumentException("O valor deve ser maior que zero.", nameof(valor));

        var diasAtraso = Math.Max(0, hoje.DayNumber - vencimento.DayNumber);
        var juros = diasAtraso == 0
            ? 0m
            : decimal.Round(valor * TaxaDiaria * diasAtraso, 2, MidpointRounding.AwayFromZero);

        return new ResultadoJuros(valor, vencimento, diasAtraso, juros, valor + juros);
    }
}
