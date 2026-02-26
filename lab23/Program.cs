using System;

namespace GoodExample {
    public interface IPrinter { void Print(); }
    public interface IScanner { void Scan(); }

    public class CanonPrinter : IPrinter {
        public void Print() => Console.WriteLine("[Canon] Друк виконано.");
    }

    // Додамо заглушку для сканера, щоб не передавати null
    public class NullScanner : IScanner {
        public void Scan() => Console.WriteLine("Сканер не підключено.");
    }

    public class MultiFunctionDevice {
        private readonly IPrinter _printer;
        private readonly IScanner _scanner;

        // Додаємо знак '?' до IScanner, щоб дозволити null, 
        // або використовуємо Default-об'єкт
        public MultiFunctionDevice(IPrinter printer, IScanner scanner) {
            _printer = printer ?? throw new ArgumentNullException(nameof(printer));
            _scanner = scanner;
        }

        public void Run() {
            _printer.Print();
            // Тепер використовуємо сканер, щоб прибрати попередження про невикористане поле
            _scanner?.Scan(); 
            Console.WriteLine("Пристрій готовий до роботи.");
        }
    }
}

class Program {
    static void Main() {
        Console.OutputEncoding = System.Text.Encoding.UTF8;

        var printer = new GoodExample.CanonPrinter();
        var scanner = new GoodExample.NullScanner(); // Замість null використовуємо об'єкт-заглушку

        var mfd = new GoodExample.MultiFunctionDevice(printer, scanner); 
        
        mfd.Run();
    }
}