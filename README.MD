# Desafio Target Sistemas

Este repositório reúne as resoluções dos desafios propostos no processo seletivo para a vaga de Desenvolvedor/a de Sistemas Jr. Cada programa está em uma pasta própria, escrito em C# (.NET 10), com a regra de negócio separada da leitura e da impressão e coberta por testes.

## Programas

### Desafio 1 — Comissão de vendas

Lê o registro de vendas do time comercial e calcula a comissão de cada vendedor. Vendas abaixo de R$ 100,00 não geram comissão, vendas abaixo de R$ 500,00 geram 1% e, a partir de R$ 500,00, a comissão é de 5%.

### Desafio 2 — Movimentação de estoque

Permite lançar entradas e saídas dos produtos do depósito. Cada movimentação recebe um identificador único e uma descrição do tipo realizado e, ao final, o programa informa a quantidade em estoque do produto movimentado.

### Desafio 3 — Juros por atraso

A partir de um valor e de uma data de vencimento, calcula os juros na data de hoje, com multa de 2,5% ao dia.


## Como rodar

É necessário o [.NET 10 SDK](https://dotnet.microsoft.com/download). Os comandos abaixo partem da raiz do repositório.

### Desafio 1 — Comissão de vendas

```powershell
dotnet run --project "desafio 1/src/ComissaoVendas/ComissaoVendas.csproj"
```

O programa lê `desafio 1/src/ComissaoVendas/Data/vendas.json` e imprime a comissão de cada vendedor. Para usar outro arquivo, informe o caminho depois de `--`:

```powershell
dotnet run --project "desafio 1/src/ComissaoVendas/ComissaoVendas.csproj" -- caminho\vendas.json
```

### Desafio 2 — Movimentação de estoque

```powershell
dotnet run --project "desafio 2/src/ControleEstoque/ControleEstoque.csproj"
```

O menu lista os produtos (`1`), lança uma movimentação (`2`) ou encerra (`0`). Em uma movimentação, informe o código do produto, `E` para entrada ou `S` para saída, a quantidade e a descrição. O estoque inicial vem de `estoque.json`. Outro arquivo pode ser passado depois de `--`.

### Desafio 3 — Juros por atraso

```powershell
dotnet run --project "desafio 3/src/CalculoJuros/CalculoJuros.csproj"
```

Escolha `1` para calcular. Informe o valor e o vencimento no formato `dd/MM/yyyy`. A opção `0` encerra o programa.

### Testes

```powershell
dotnet test "desafio 1"
dotnet test "desafio 2"
dotnet test "desafio 3"
```
##

#### Desenvolvido por: Douglas Alisson (DevDouglas)