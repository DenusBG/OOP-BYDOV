using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        List<Shape> shapes = new List<Shape>
        {
            new Circle(5.0),
            new Rectangle(4.0, 6.0),
            new Circle(10.0),
            new Rectangle(8.0, 3.0)
        };

        Console.WriteLine("--- Інформація про всі фігури ---");
        foreach (var shape in shapes)
        {
            shape.DisplayInfo();
            Console.WriteLine();
        }

        Shape largestShape = shapes.OrderByDescending(s => s.Area()).First();

        Console.WriteLine("--- Фігура з найбільшою площею ---");
        largestShape.DisplayInfo();
    }
}