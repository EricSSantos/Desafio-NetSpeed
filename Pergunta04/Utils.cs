using System;
using System.Collections.Generic;

class Utils
{
    public static bool VerificarFinalDeSemana(DateTime data)
    {
        return data.DayOfWeek == DayOfWeek.Saturday || data.DayOfWeek == DayOfWeek.Sunday;
    }

    public static DateTime VerificarProximoDiaUtil(DateTime data)
    {
        data = data.AddDays(1);
        while (VerificarFinalDeSemana(data) || VerificarFeriado(data))
        {
            data = data.AddDays(1);
        }
        return data;
    }

    public static bool VerificarFeriado(DateTime data)
    {
        List<DateTime> feriados = new List<DateTime>
        {
            new DateTime(data.Year, 1, 1),    // Ano Novo
            new DateTime(data.Year, 4, 21),   // Tiradentes
            new DateTime(data.Year, 5, 1),    // Dia do Trabalho
            new DateTime(data.Year, 9, 7),    // Independência
            new DateTime(data.Year, 10, 12),  // Nossa Senhora Aparecida
            new DateTime(data.Year, 11, 15),  // Proclamação da República
            new DateTime(data.Year, 12, 25),  // Natal
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
}
