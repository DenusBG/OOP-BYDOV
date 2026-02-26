using System;
using System.Collections.Generic;

namespace lab24
{
    // === ПАТЕРН STRATEGY ===
    public interface INumericOperationStrategy
    {
        double Execute(double value);
        string Name { get; }
    }

    public class SquareOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => value * value;
        public string Name => "Квадрат числа";
    }

    public class CubeOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => value * value * value;
        public string Name => "Куб числа";
    }

    public class SquareRootOperationStrategy : INumericOperationStrategy
    {
        public double Execute(double value) => Math.Sqrt(value);
        public string Name => "Квадратний корінь";
    }

    public class NumericProcessor
    {
        private INumericOperationStrategy _strategy;

        public NumericProcessor(INumericOperationStrategy strategy) => _strategy = strategy;

        public void SetStrategy(INumericOperationStrategy strategy) => _strategy = strategy;

        public double Process(double input) => _strategy.Execute(input);

        public string GetCurrentStrategyName() => _strategy.Name;
    }

    // === ПАТЕРН OBSERVER (Subject) ===
    public class ResultPublisher
    {
        public event Action<double, string>? ResultCalculated;

        public void PublishResult(double result, string operationName)
        {
            ResultCalculated?.Invoke(result, operationName);
        }
    }

    // === СПОСТЕРІГАЧІ (Observers) ===
    public class ConsoleLoggerObserver
    {
        public void OnResultCalculated(double result, string opName) 
            => Console.WriteLine($"[ConsoleLogger] Операція '{opName}': Результат = {result:F2}");
    }

    public class HistoryLoggerObserver
    {
        public List<string> History { get; } = new List<string>();
        public void OnResultCalculated(double result, string opName) 
            => History.Add($"{DateTime.Now:T}: {opName} = {result:F2}");
    }

    public class ThresholdNotifierObserver
    {
        private readonly double _threshold;
        public ThresholdNotifierObserver(double threshold) => _threshold = threshold;

        public void OnResultCalculated(double result, string opName)
        {
            if (result > _threshold)
                Console.WriteLine($"[ALERT] Результат {result:F2} перевищує поріг {_threshold}!");
        }
    }

    // === MAIN ===
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // 1. Ініціалізація
            var publisher = new ResultPublisher();
            var processor = new NumericProcessor(new SquareOperationStrategy());

            // 2. Налаштування спостерігачів
            var consoleObs = new ConsoleLoggerObserver();
            var historyObs = new HistoryLoggerObserver();
            var alertObs = new ThresholdNotifierObserver(100.0);

            publisher.ResultCalculated += consoleObs.OnResultCalculated;
            publisher.ResultCalculated += historyObs.OnResultCalculated;
            publisher.ResultCalculated += alertObs.OnResultCalculated;

            // 3. Демонстрація роботи
            double[] inputs = { 5, 12, 144 };

            Console.WriteLine("--- Виконання операцій ---");
            
            // Квадрат
            PerformTask(processor, publisher, 5);

            // Куб (зміна стратегії)
            processor.SetStrategy(new CubeOperationStrategy());
            PerformTask(processor, publisher, 5);

            // Корінь (зміна стратегії)
            processor.SetStrategy(new SquareRootOperationStrategy());
            PerformTask(processor, publisher, inputs[2]);

            // Вивід історії
            Console.WriteLine("\n--- Історія операцій ---");
            historyObs.History.ForEach(Console.WriteLine);
        }

        static void PerformTask(NumericProcessor p, ResultPublisher pub, double val)
        {
            double res = p.Process(val);
            pub.PublishResult(res, p.GetCurrentStrategyName());
        }
    }
}