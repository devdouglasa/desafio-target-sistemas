namespace ComissaoVendas.Models;

public sealed record ResumoVendedor(
    string Vendedor,
    int QuantidadeVendas,
    decimal TotalVendido,
    decimal TotalComissao);
