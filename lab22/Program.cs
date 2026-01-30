using System;

namespace lab22
{
    // ==========================================
    // Частина 1: ПОРУШЕННЯ LSP
    // ==========================================
    public class Rectangle
    {
        public virtual int Width { get; set; }
        public virtual int Height { get; set; }

        public int CalculateArea() => Width * Height;
    }

    public class Square : Rectangle
    {
        // Порушення: Зміна однієї властивості неявно змінює іншу
        public override int Width
        {
            set { base.Width = base.Height = value; }
        }

        public override int Height
        {
            set { base.Width = base.Height = value; }
        }
    }

    // ==========================================
    // Частина 2: РЕФАКТОРИНГ (Дотримання LSP)
    // Використовуємо спільний інтерфейс замість наслідування
    // ==========================================
    public interface IShape
    {
        int CalculateArea();
    }

    public class ValidRectangle : IShape
    {
        public int Width { get; set; }
        public int Height { get; set; }
        public int CalculateArea() => Width * Height;
    }

    public class ValidSquare : IShape
    {
        public int Side { get; set; }
        public int CalculateArea() => Side * Side;
    }

    class Program
    {
        // Клієнтський метод, що демонструє проблему LSP
        static void TestRectangleArea(Rectangle rect)
        {
            rect.Width = 5;
            rect.Height = 10;

            // Клієнт очікує площу 50 (5 * 10)
            Console.WriteLine($"Очікувана площа: 50, Фактична: {rect.CalculateArea()}");
            
            if (rect.CalculateArea() != 50)
            {
                Console.WriteLine("!!! Помилка: Поведінка об'єкта не відповідає контракту Rectangle.");
            }
        }

        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            Console.WriteLine("--- Демонстрація порушення LSP ---");
            Rectangle badSquare = new Square();
            TestRectangleArea(badSquare); 

            Console.WriteLine("\n--- Демонстрація після рефакторингу ---");
            IShape shape1 = new ValidRectangle { Width = 5, Height = 10 };
            IShape shape2 = new ValidSquare { Side = 5 };

            Console.WriteLine($"Площа прямокутника (5x10): {shape1.CalculateArea()}");
            Console.WriteLine($"Площа квадрата (сторона 5): {shape2.CalculateArea()}");
        }
    }
}