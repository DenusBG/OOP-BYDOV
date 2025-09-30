using System;

namespace Lab1Project
{
    class Program
    {
        static void Main(string[] args)
        {
            // Створюємо об'єкт Figure
            Figure f = new Figure(25.5);
            
            // Виводимо інформацію про фігуру
            Console.WriteLine(f.GetFigure());
        }
    }
}
