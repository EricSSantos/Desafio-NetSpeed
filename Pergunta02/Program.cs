using System;

class Program
{
    static void Main()
    {
        while (true)
        {
            int codigoAluno = LerCodigoAluno();
            if (codigoAluno == 0)
                break;

            (double nota1, double nota2, double nota3) = LerNotas();

            double mediaPonderada = CalcularMediaPonderada(nota1, nota2, nota3);

            ExibirInformacoesAluno(codigoAluno, nota1, nota2, nota3, mediaPonderada);

            VerificarAprovacao(mediaPonderada);
        }

        Console.WriteLine("Encerrando o programa...");
    }

    static int LerCodigoAluno()
    {
        Console.Write("Digite o código do aluno (0 para encerrar): ");
        return int.Parse(Console.ReadLine());
    }

    static (double, double, double) LerNotas()
    {
        Console.Write("Digite a primeira nota: ");
        double nota1 = double.Parse(Console.ReadLine());

        Console.Write("Digite a segunda nota: ");
        double nota2 = double.Parse(Console.ReadLine());

        Console.Write("Digite a terceira nota: ");
        double nota3 = double.Parse(Console.ReadLine());

        return (nota1, nota2, nota3);
    }

    static double CalcularMediaPonderada(double nota1, double nota2, double nota3)
    {
        double maiorNota = Math.Max(nota1, Math.Max(nota2, nota3));

        double somaNotas = nota1 + nota2 + nota3;
        double somaMenores = somaNotas - maiorNota;

        return (maiorNota * 4 + somaMenores * 3) / 10;
    }

    static void ExibirInformacoesAluno(int codigoAluno, double nota1, double nota2, double nota3, double mediaPonderada)
    {
        Console.WriteLine($"\nCódigo do aluno: {codigoAluno}");
        Console.WriteLine($"Notas: {nota1}, {nota2}, {nota3}");
        Console.WriteLine($"Média ponderada: {mediaPonderada}");
    }

    static void VerificarAprovacao(double mediaPonderada)
    {
        Console.WriteLine(mediaPonderada >= 6 ? "APROVADO\n" : "REPROVADO\n");
    }
}
