using System;

class Program
{
    static void Main()
    {
        double totalDesconto = 0;
        double totalValorPago = 0;
        int carrosAte2000 = 0;
        int totalCarros = 0;

        while (true)
        {
            (int anoVeiculo, double valorVeiculo) = ObterInformacoesVeiculo();

            double desconto = CalcularDesconto(anoVeiculo, valorVeiculo);
            double valorPago = valorVeiculo - desconto;
            ExibirDescontoValor(valorPago, desconto);

            totalDesconto += desconto;
            totalValorPago += valorPago;
            totalCarros++;
            if (anoVeiculo <= 2000)
                carrosAte2000++;

            if (!DesejaContinuar())
                break;
        }

        ExibirTotais(carrosAte2000, totalCarros, totalDesconto, totalValorPago);
    }

    static (int, double) ObterInformacoesVeiculo()
    {
        Console.Write("Digite o ano do veículo: ");
        int anoVeiculo = int.Parse(Console.ReadLine());

        Console.Write("Digite o valor do veículo: ");
        double valorVeiculo = double.Parse(Console.ReadLine());

        return (anoVeiculo, valorVeiculo);
    }

    static double CalcularDesconto(int anoVeiculo, double valorVeiculo)
    {
        return (anoVeiculo <= 2000) ? valorVeiculo * 0.12 : valorVeiculo * 0.07;
    }

    static void ExibirDescontoValor(double valorPago, double desconto)
    {
        Console.WriteLine($"================================================================");
        Console.WriteLine($"Desconto: R$ {desconto}");
        Console.WriteLine($"Valor a pagar: R$ {valorPago}");
        Console.WriteLine($"================================================================");
    }

    static bool DesejaContinuar()
    {
        Console.Write("Deseja continuar calculando desconto? (S/N): ");
        string continuar = Console.ReadLine().ToUpper();

        return continuar == "S";
    }

    static void ExibirTotais(int carrosAte2000, int totalCarros, double totalDesconto, double totalValorPago)
    {
        Console.WriteLine($"================================================================");
        Console.WriteLine($"Total de carros com ano até 2000: {carrosAte2000}");
        Console.WriteLine($"Total de carros vendidos: {totalCarros}");
        Console.WriteLine($"Total de desconto concedido: R$ {totalDesconto}");
        Console.WriteLine($"Total a pagar pelos carros: R$ {totalValorPago}");
        Console.WriteLine($"================================================================");
    }
}
