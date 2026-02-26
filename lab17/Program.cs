using System;

namespace lab17.FactoryExample
{
    // Product: Інтерфейс для логера
    public interface ILogger {
        void Log(string message);
    }

    // ConcreteProduct A: Консольний логер
    public class ConsoleLogger : ILogger {
        public void Log(string message) => Console.WriteLine($"[Console] {message}");
    }

    // ConcreteProduct B: Файловий логер
    public class FileLogger : ILogger {
        public void Log(string message) => Console.WriteLine($"[File] (імітація запису): {message}");
    }

    // Creator: Абстрактний клас з фабричним методом
    public abstract class LoggerFactory {
        public abstract ILogger CreateLogger();
        
        public void LogMessage(string message) {
            ILogger logger = CreateLogger(); // Використання фабричного методу
            logger.Log(message);
        }
    }

    // ConcreteCreator A: Фабрика консолі
    public class ConsoleLoggerFactory : LoggerFactory {
        public override ILogger CreateLogger() => new ConsoleLogger();
    }

    // ConcreteCreator B: Фабрика файлів
    public class FileLoggerFactory : LoggerFactory {
        public override ILogger CreateLogger() => new FileLogger();
    }

    class Program {
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            Console.WriteLine("=== Демонстрація Factory Method ===");

            LoggerFactory consoleFactory = new ConsoleLoggerFactory();
            consoleFactory.LogMessage("Повідомлення для консолі.");

            LoggerFactory fileFactory = new FileLoggerFactory();
            fileFactory.LogMessage("Повідомлення для файлу.");
        }
    }
}