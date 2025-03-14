using System;
using System.Collections.Generic;

class Program
{
    const decimal JUROS_POR_DIA = 0.03m;
    const decimal MULTA_FIXA = 2.00m;

    static void Main()
    {
        Console.WriteLine("Deseja usar os valores de teste (1) ou digitar seus próprios valores (2)?");
        string opcao = Console.ReadLine();

        if (opcao == "1")
        {
            var testes = new (DateTime vencimento, decimal valor, DateTime pagamento)[]
            {
                (new DateTime(2023, 5, 6), 100, new DateTime(2023, 5, 8)),
                (new DateTime(2023, 5, 7), 100, new DateTime(2023, 5, 9)),
                (new DateTime(2023, 5, 1), 100, new DateTime(2023, 5, 2)),
                (new DateTime(2023, 4, 21), 100, new DateTime(2023, 4, 24)),
                (new DateTime(2023, 4, 7), 100, new DateTime(2023, 4, 11)),
                (new DateTime(2023, 5, 10), 100, new DateTime(2023, 5, 10)),
                (new DateTime(2023, 5, 11), 100, new DateTime(2023, 5, 10)),
                (new DateTime(2023, 5, 8), 100, new DateTime(2023, 5, 9))
            };

            foreach (var teste in testes)
            {
                decimal valorRecalculado, valorJuros;
                CalcularBoletoRecalculado(teste.vencimento, teste.pagamento, teste.valor, out valorRecalculado, out valorJuros);
                Console.WriteLine($"Vencimento: {teste.vencimento:yyyy-MM-dd}, Pagamento: {teste.pagamento:yyyy-MM-dd}, Valor original: R$ {teste.valor:F2}");
                Console.WriteLine($"Valor recalculado: R$ {valorRecalculado:F2}, Juros totais: R$ {valorJuros:F2}\n");
            }
        }
        else if (opcao == "2")
        {
            Console.Write("Digite a data de vencimento (yyyy-MM-dd): ");
            DateTime vencimento = DateTime.Parse(Console.ReadLine());

            Console.Write("Digite o valor original do boleto: R$ ");
            decimal valor = decimal.Parse(Console.ReadLine());

            Console.Write("Digite a data de pagamento (yyyy-MM-dd): ");
            DateTime pagamento = DateTime.Parse(Console.ReadLine());

            decimal valorRecalculado, valorJuros;
            CalcularBoletoRecalculado(vencimento, pagamento, valor, out valorRecalculado, out valorJuros);
            Console.WriteLine($"Vencimento: {vencimento:yyyy-MM-dd}, Pagamento: {pagamento:yyyy-MM-dd}, Valor original: R$ {valor:F2}");
            Console.WriteLine($"Valor recalculado: R$ {valorRecalculado:F2}, Juros totais: R$ {valorJuros:F2}\n");
        }
        else
        {
            Console.WriteLine("Opção inválida. Por favor, reinicie o programa.");
        }
    }

    static void CalcularBoletoRecalculado(DateTime dataVencimento, DateTime dataPagamento, decimal valorBoleto, out decimal valorRecalculado, out decimal valorJuros)
    {
        valorRecalculado = valorBoleto;
        valorJuros = 0;

        if ((dataPagamento == dataVencimento) || (dataPagamento < dataVencimento))
            return;

        if (ValidarCondicoesPagamento(dataVencimento, dataPagamento, out valorJuros))
        {
            valorRecalculado = valorBoleto + valorJuros;
            return;
        }
    }

    private static bool ValidarCondicoesPagamento(DateTime dataVencimento, DateTime dataPagamento, out decimal valorJuros)
    {
        valorJuros = 0;

        bool dataVencimentoIsFeriado = VerificaFeriado(dataVencimento);
        bool dataVencimentoIsFinalDeSemana = VerificaFinalDeSemana(dataVencimento);
        DateTime proximoDiaUtil = ObterProximoDiaUtil(dataVencimento);


        if (dataVencimentoIsFeriado && !dataVencimentoIsFinalDeSemana)
        {
            return ValidarParaFeriadoSemFinalDeSemana(dataVencimento, dataPagamento, out valorJuros);
        }

        if (dataVencimentoIsFeriado || dataVencimentoIsFinalDeSemana)
        {
            return ValidarParaFeriadoOuFinalDeSemana(dataVencimento, dataPagamento, proximoDiaUtil, dataVencimentoIsFeriado, out valorJuros);
        }

        return ValidarParaDiaUtilComum(dataVencimento, dataPagamento, out valorJuros);
    }

    private static bool ValidarParaFeriadoSemFinalDeSemana(DateTime dataVencimento, DateTime dataPagamento, out decimal valorJuros)
    {
        DateTime primeiroDiaUtil = ObterProximoDiaUtil(dataVencimento);
        DateTime segundoDiaUtil = ObterProximoDiaUtil(primeiroDiaUtil);

        if (dataPagamento == segundoDiaUtil)
        {
            int diasCorridos = (dataPagamento - dataVencimento).Days;
            valorJuros = CalcularJuros(dataVencimento, dataPagamento, diasCorridos);
            return true;
        }

        valorJuros = 0;
        return false;
    }

    private static bool ValidarParaFeriadoOuFinalDeSemana(DateTime dataVencimento, DateTime dataPagamento, DateTime proximoDiaUtil, bool dataVencimentoIsFeriado, out decimal valorJuros)
    {
        if (dataPagamento == proximoDiaUtil)
        {
            valorJuros = 0;
            return false;
        }

        if (dataVencimentoIsFeriado && VerificaFinalDeSemana(dataVencimento.AddDays(1)))
        {
            DateTime segundaFeira = ObterProximoDiaUtil(dataVencimento.AddDays(2));
            if (dataPagamento == segundaFeira)
            {
                valorJuros = 0;
                return false;
            }
        }

        if (PosteriorAoDiaUtilConsecutivo(dataPagamento, dataVencimento))
        {
            valorJuros = CalcularJuros(dataVencimento, dataPagamento, ((dataPagamento - dataVencimento).Days + 1));
            return true;
        }

        valorJuros = 0;
        return false;
    }

    private static bool ValidarParaDiaUtilComum(DateTime dataVencimento, DateTime dataPagamento, out decimal valorJuros)
    {
        valorJuros = CalcularJuros(dataVencimento, dataPagamento, (dataPagamento - dataVencimento).Days);
        return true;
    }

    private static decimal CalcularJuros(DateTime dataVencimento, DateTime dataPagamento, int diasAtraso)
    {
        decimal juros = (diasAtraso * JUROS_POR_DIA);
        return juros + MULTA_FIXA;
    }

    static bool VerificaFinalDeSemana(DateTime data)
    {
        return data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;
    }

    static bool VerificaFeriado(DateTime data)
    {
        List<DateTime> feriados = new List<DateTime>
        {
            new DateTime(data.Year, 1, 1),    // Ano Novo
            new DateTime(data.Year, 4, 21),   // Tiradentes
            new DateTime(data.Year, 5, 1),    // Dia do Trabalho
            new DateTime(data.Year, 9, 7),    // Independência do Brasil
            new DateTime(data.Year, 10, 12),  // Nossa Senhora Aparecida
            new DateTime(data.Year, 11, 2),   // Finados
            new DateTime(data.Year, 11, 15),  // Proclamação da República
            new DateTime(data.Year, 12, 25)   // Natal
        };

        DateTime pascoa = VerificarPascoa(data.Year);
        DateTime sextaFeiraSanta = pascoa.AddDays(-2);

        feriados.Add(sextaFeiraSanta);

        return feriados.Contains(data.Date);
    }

    public static DateTime VerificarPascoa(int ano)
    {
        int a = ano % 19;
        int b = ano / 100;
        int c = ano % 100;
        int d = b / 4;
        int e = b % 4;
        int f = (b + 8) / 25;
        int g = (b - f + 1) / 3;
        int h = (19 * a + b - d - g + 15) % 30;
        int i = c / 4;
        int k = c % 4;
        int l = (32 + 2 * e + 2 * i - h - k) % 7;
        int m = (a + 11 * h + 22 * l) / 451;
        int month = (h + l - 7 * m + 114) / 31;
        int day = ((h + l - 7 * m + 114) % 31) + 1;

        return new DateTime(ano, month, day);
    }

    static DateTime ObterProximoDiaUtil(DateTime data)
    {
        DateTime proximoDia = data.AddDays(1);
        while (VerificaFeriado(proximoDia) || VerificaFinalDeSemana(proximoDia))
        {
            proximoDia = proximoDia.AddDays(1);
        }
        return proximoDia;
    }

    static bool PosteriorAoDiaUtilConsecutivo(DateTime dataPagamento, DateTime dataVencimento)
    {
        DateTime proximoDiaUtil = ObterProximoDiaUtil(dataVencimento);
        return dataPagamento > proximoDiaUtil;
    }
}
