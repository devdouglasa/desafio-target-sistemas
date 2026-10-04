namespace ComissaoVendas.Services;

public static class CalculadoraComissao
{
    public static decimal Calcular(decimal valor)
    {
        if (valor < 100m)
            return 0m;

        var aliquota = valor < 500m ? 0.01m : 0.05m;
        return decimal.Round(valor * aliquota, 2, MidpointRounding.AwayFromZero);
    }
}
