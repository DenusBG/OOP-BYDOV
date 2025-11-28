// Program.cs
using System;
using System.Collections.Generic;
using System.Linq; 

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторна робота №4: Калькулятор Площ Фігур ===");
        Console.WriteLine("Тема: Абстракції, Інтерфейси, Композиція та Агрегація.");

        // 1. Створення різних об'єктів
        var circleA = new Circle(radius: 5.0);        
        var rectangleB = new Rectangle(width: 4.0, height: 6.0); 
        var circleC = new Circle(radius: 2.0);        
        var rectangleD = new Rectangle(width: 10.0, height: 8.0); 

        // 2. Створення колекції IArea
        List<IArea> shapes = new List<IArea> 
        { 
            circleA, 
            rectangleB, 
            circleC, 
            rectangleD 
        };

        // 3. Створення AreaProcessor (Агрегація)
        var processor = new AreaProcessor(shapes);

        Console.WriteLine("\n--- Деталі індивідуальних фігур ---");
        foreach (var shape in shapes)
        {
            if (shape is BaseShape bs)
            {
                Console.WriteLine($"[Фігура: {bs.Name}] ({bs.GetParameters()}) | Площа: {shape.CalculateArea():F2}");
            }
        }

        // 4. Демонстрація загальних обчислень
        double totalArea = processor.CalculateTotalArea();
        Console.WriteLine($"\n--- Результати обробки (AreaProcessor) ---");
        Console.WriteLine($"Сумарна площа всіх фігур: {totalArea:F2}");

        // Пошук min/max
        IArea minShape = processor.FindMinAreaShape();
        IArea maxShape = processor.FindMaxAreaShape();

        // Виведення результатів пошуку
        Console.WriteLine($"\n**Мінімальна площа ({minShape.CalculateArea():F2})** належить: {(minShape as BaseShape)?.Name} ({(minShape as BaseShape)?.GetParameters()})");
        Console.WriteLine($"**Максимальна площа ({maxShape.CalculateArea():F2})** належить: {(maxShape as BaseShape)?.Name} ({(maxShape as BaseShape)?.GetParameters()})");
    }
}