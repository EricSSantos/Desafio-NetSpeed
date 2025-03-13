using System;

class Program
{
    static void Main()
    {
        char[] alternativas = { 'A', 'B', 'C', 'D', 'E' };
        int[,] testes = {
            {1, 2, 3},  // A
            {3, 4, 5},  // B
            {2, 2, 4},  // C
            {4, 4, 4},  // D
            {5, 3, 3}   // E
        };

        for (int i = 0; i < testes.GetLength(0); i++)
        {
            int a = testes[i, 0];
            int b = testes[i, 1];
            int c = testes[i, 2];

            string resultado = ClassificarTriangulo(a, b, c);

            Console.WriteLine($" {alternativas[i]}) a = {a}, b = {b}, c = {c} \n RESULTADO: {resultado} \n");
        }
    }

    static string ClassificarTriangulo(int a, int b, int c)
    {
        if ((a < b + c) && (b < a + c) && (c < a + b))
        {
            if ((a == b) && (b == c))
                return "Triângulo Equilátero";
            else if ((a == b) || (b == c) || (a == c))
                return "Triângulo Isósceles";
            else
                return "Triângulo Escaleno";
        }
        return "Não é possível formar um triângulo";
    }
}
