using System;
using System.Collections.Generic;

class Program
{
    const decimal VALOR_JUROS_DIA = 0.03m;
    const decimal VALOR_MULTA = 2.00m;

    static void Main()
    {
        Console.Write("Deseja informar os valores ou realizar os testes fixos? (1 - Informar / 2 - Testes): ");
        int opcao = Convert.ToInt32(Console.ReadLine());

        if (opcao == 1)
        {
            DateTime vencimentoOriginal;
            decimal valorBoletoOriginal;
            DateTime dataPagamento;

            Console.Write("Digite a data de vencimento original (dd/MM/yyyy): ");
            while (!DateTime.TryParse(Console.ReadLine(), out vencimentoOriginal))
            {
                Console.Write("Data inválida! Digite novamente: ");
            }

            Console.Write("Digite o valor do boleto original (R$): ");
            while (!decimal.TryParse(Console.ReadLine(), out valorBoletoOriginal) || valorBoletoOriginal <= 0)
            {
                Console.Write("Valor inválido! Digite novamente: ");
            }

            Console.Write("Digite a data de pagamento (dd/MM/yyyy): ");
            while (!DateTime.TryParse(Console.ReadLine(), out dataPagamento))
            {
                Console.Write("Data inválida! Digite novamente: ");
            }

            CalcularValorBoleto(vencimentoOriginal, valorBoletoOriginal, dataPagamento);
        }
        else if (opcao == 2)
        {
            Testes();
        }
        else
        {
            Console.WriteLine("Opção inválida. O programa será encerrado.");
        }
    }

    static void CalcularValorBoleto(DateTime vencimentoOriginal, decimal valorBoletoOriginal, DateTime dataPagamento)
    {
        int diasDeAtraso = (dataPagamento - vencimentoOriginal).Days;
        bool isFinalDeSemana = Utils.VerificarFinalDeSemana(vencimentoOriginal);
        bool isFeriado = Utils.VerificarFeriado(vencimentoOriginal);
        DateTime primeiroDiaUtilAposVencimento = vencimentoOriginal;
        DateTime? proximoDiaUtil = isFinalDeSemana || isFeriado ? Utils.VerificarProximoDiaUtil(vencimentoOriginal) : (DateTime?)null;
        var resultadoPagamento = (dataPagamento, isFinalDeSemana, isFeriado, proximoDiaUtil);

        Action calcularJuros = () =>
        {
            diasDeAtraso = (dataPagamento - vencimentoOriginal).Days + 1;
            CalcularValorTotalBoleto(valorBoletoOriginal, diasDeAtraso);
        };

        switch (resultadoPagamento)
        {
            case var _ when dataPagamento == vencimentoOriginal:
                Console.WriteLine("Pagamento realizado no mesmo dia do vencimento. Não há juros nem multa.");
                return;

            case var _ when dataPagamento < vencimentoOriginal:
                Console.WriteLine("Pagamento realizado antes do vencimento. Não há juros nem multa.");
                return;

            case var _ when proximoDiaUtil.HasValue && dataPagamento == proximoDiaUtil.Value:
                Console.WriteLine("Pagamento realizado no próximo dia útil após o vencimento. Não há juros nem multa.");
                return;

            case var _ when proximoDiaUtil.HasValue && dataPagamento > proximoDiaUtil.Value:
                calcularJuros();
                return;

            case var _ when vencimentoOriginal.DayOfWeek != DayOfWeek.Saturday && vencimentoOriginal.DayOfWeek != DayOfWeek.Sunday && dataPagamento == vencimentoOriginal.AddDays(1):
                diasDeAtraso = 1;
                CalcularValorTotalBoleto(valorBoletoOriginal, diasDeAtraso);
                return;

            default:
                Console.WriteLine("Pagamento realizado após vencimento, com juros.");
                calcularJuros();
                return;
        }
    }

    static decimal CalcularValorTotalBoleto(decimal valorBoletoOriginal, int diasDeAtraso)
    {
        decimal valorTotalJuros = diasDeAtraso * VALOR_JUROS_DIA;
        decimal valorFinal = valorBoletoOriginal + valorTotalJuros + VALOR_MULTA;

        Console.WriteLine($"O valor total do boleto é: R$ {valorFinal:F2} (Juros: R$ {valorTotalJuros:F2}, Multa: R$ {VALOR_MULTA:F2})");
        return valorFinal;
    }

    static void Testes()
    {
        List<(DateTime vencimento, decimal valorBoleto, DateTime pagamento)> casosDeTeste = new List<(DateTime, decimal, DateTime)>
        {
            (new DateTime(2023, 5, 6), 100, new DateTime(2023, 5, 8)),
            (new DateTime(2023, 5, 7), 100, new DateTime(2023, 5, 9)),
            (new DateTime(2023, 5, 1), 100, new DateTime(2023, 5, 2)),
            (new DateTime(2023, 4, 21), 100, new DateTime(2023, 4, 24)),
            (new DateTime(2023, 4, 7), 100, new DateTime(2023, 4, 11)),
            (new DateTime(2023, 5, 10), 100, new DateTime(2023, 5, 10)),
            (new DateTime(2023, 5, 11), 100, new DateTime(2023, 5, 10)),
            (new DateTime(2023, 5, 8), 100, new DateTime(2023, 5, 9)),
        };

        int numeroTeste = 1;
        foreach (var caso in casosDeTeste)
        {
            Console.WriteLine($"\n{numeroTeste}: " +
                $"Vencimento: {caso.vencimento.ToString("dd/MM/yyyy")} || " +
                $"Valor Original: R$ {caso.valorBoleto:F2} || " +
                $"Pagamento: {caso.pagamento.ToString("dd/MM/yyyy")}");

            CalcularValorBoleto(caso.vencimento, caso.valorBoleto, caso.pagamento);
            numeroTeste++;
            Console.WriteLine();
        }
    }
}
