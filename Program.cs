using System;

// Цей клас 'Program' є обов'язковим контейнером
class Program
{
    // Ось вона, ТОЧКА ВХОДУ. 
    // Без цього методу програма не знає, звідки стартувати.
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        Console.WriteLine("--- Демонстрація роботи класу Polynomial ---\n");

        // 1. Створення об'єктів
        Polynomial p1 = new Polynomial(5, -3, 2);
        Polynomial p2 = new Polynomial(-1, 4, 0, 1);

        Console.WriteLine($"Створено перший многочлен p1(x) = {p1}");
        Console.WriteLine($"Створено другий многочлен p2(x) = {p2}\n");

        // 2. Використання індексатора
        Console.WriteLine("--- Використання індексатора ---");
        Console.WriteLine($"Коефіцієнт при x^1 у p1: p1[1] = {p1[1]}");

        p1[1] = 10;
        Console.WriteLine($"Змінили коефіцієнт при x^1 на 10. Тепер p1(x) = {p1}\n");

        // 3. Використання оператора '+'
        Console.WriteLine("--- Використання перевантаженого оператора '+' ---");
        Polynomial p_sum = p1 + p2;

        Console.WriteLine($"Сума многочленів ({p1}) + ({p2})");
        Console.WriteLine($"Результат: p_sum(x) = {p_sum}");
        
        Console.WriteLine($"\nСтепінь многочлена p_sum: {p_sum.Degree}");
    }
}