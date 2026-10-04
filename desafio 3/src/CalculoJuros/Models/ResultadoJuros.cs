namespace CalculoJuros.Models;

public sealed record ResultadoJuros(
    decimal ValorOriginal,
    DateOnly Vencimento,
    int DiasAtraso,
    decimal Juros,
    decimal ValorAtualizado);
