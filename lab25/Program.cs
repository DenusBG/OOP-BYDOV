using System;
using System.Collections.Generic;

namespace lab25
{
    // --- 1. FACTORY METHOD & LOGGER ---
    public interface ILogger { void Log(string message); }
    
    public class ConsoleLogger : ILogger {
        public void Log(string message) => Console.WriteLine($"[CONSOLE LOG]: {message}");
    }

    public class FileLogger : ILogger {
        public void Log(string message) => Console.WriteLine($"[FILE LOG (імітація)]: {message}");
    }

    public abstract class LoggerFactory {
        public abstract ILogger CreateLogger();
    }

    public class ConsoleLoggerFactory : LoggerFactory {
        public override ILogger CreateLogger() => new ConsoleLogger();
    }

    public class FileLoggerFactory : LoggerFactory {
        public override ILogger CreateLogger() => new FileLogger();
    }

    // --- 2. SINGLETON (LoggerManager) ---
    public sealed class LoggerManager {
        private static LoggerManager _instance = null!;
        private static readonly object _lock = new object();
        private LoggerFactory _factory;

        private LoggerManager(LoggerFactory factory) => _factory = factory;

        public static void Initialize(LoggerFactory factory) {
            lock (_lock) {
                if (_instance == null) _instance = new LoggerManager(factory);
            }
        }

        public static LoggerManager Instance {
            get {
                if (_instance == null) throw new Exception("LoggerManager не ініціалізовано!");
                return _instance;
            }
        }

        public void SetFactory(LoggerFactory factory) => _factory = factory;
        public void Log(string message) => _factory.CreateLogger().Log(message);
    }

    // --- 3. STRATEGY (Data Processing) ---
    public interface IDataProcessorStrategy {
        string Process(string data);
        string Name { get; }
    }

    public class EncryptDataStrategy : IDataProcessorStrategy {
        public string Process(string data) => $"<Encrypted>{data}</Encrypted>";
        public string Name => "Шифрування";
    }

    public class CompressDataStrategy : IDataProcessorStrategy {
        public string Process(string data) => $"<Compressed>{data}</Compressed>";
        public string Name => "Стиснення";
    }

    public class DataContext {
        private IDataProcessorStrategy _strategy;
        public DataContext(IDataProcessorStrategy strategy) => _strategy = strategy;
        public void SetStrategy(IDataProcessorStrategy strategy) => _strategy = strategy;
        public string ExecuteStrategy(string data) => _strategy.Process(data);
        public string GetStrategyName() => _strategy.Name;
    }

    // --- 4. OBSERVER (DataPublisher) ---
    public class DataPublisher {
        public event Action<string, string>? DataProcessed;
        public void Publish(string result, string opName) => DataProcessed?.Invoke(result, opName);
    }

    public class ProcessingLoggerObserver {
        public void OnDataProcessed(string result, string opName) {
            LoggerManager.Instance.Log($"Спостерігач зафіксував: операція '{opName}', результат: {result}");
        }
    }

    // --- MAIN: СЦЕНАРІЇ ---
    class Program {
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Ініціалізація Singleton логера
            LoggerManager.Initialize(new ConsoleLoggerFactory());

            // Сценарій 1: Повна інтеграція
            PrintHeader("Сценарій 1: Повна інтеграція");
            var context = new DataContext(new EncryptDataStrategy());
            var publisher = new DataPublisher();
            var observer = new ProcessingLoggerObserver();
            
            publisher.DataProcessed += observer.OnDataProcessed;

            string data = "Секретні дані";
            string result = context.ExecuteStrategy(data);
            publisher.Publish(result, context.GetStrategyName());

            // Сценарій 2: Динамічна зміна логера
            PrintHeader("Сценарій 2: Динамічна зміна логера");
            LoggerManager.Instance.SetFactory(new FileLoggerFactory());
            result = context.ExecuteStrategy("Інші дані");
            publisher.Publish(result, context.GetStrategyName());

            // Сценарій 3: Динамічна зміна стратегії
            PrintHeader("Сценарій 3: Динамічна зміна стратегії");
            context.SetStrategy(new CompressDataStrategy());
            result = context.ExecuteStrategy("Велика база даних");
            publisher.Publish(result, context.GetStrategyName());
        }

        static void PrintHeader(string text) {
            Console.WriteLine($"\n{new string('=', 10)} {text} {new string('=', 10)}");
        }
    }
}