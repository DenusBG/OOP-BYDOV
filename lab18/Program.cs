using System;
using System.Collections.Generic;

namespace lab18.Patterns {
    // --- ПАТЕРН STRATEGY ---
    public interface ISortStrategy {
        void Sort(List<int> data);
    }

    public class QuickSortStrategy : ISortStrategy {
        public void Sort(List<int> data) {
            Console.WriteLine("[Strategy] Сортування через Quick Sort...");
            data.Sort(); 
        }
    }

    public class Sorter {
        private ISortStrategy _strategy;
        public Sorter(ISortStrategy strategy) => _strategy = strategy;
        public void PerformSort(List<int> data) => _strategy.Sort(data);
    }

    // --- ПАТЕРН OBSERVER ---
    public interface IObserver {
        void Update(string message);
    }

    public class ConsoleDisplay : IObserver {
        public void Update(string message) => Console.WriteLine($"[Observer] Отримано сповіщення: {message}");
    }

    public class StockMonitor {
        private List<IObserver> _observers = new List<IObserver>();
        private decimal _price;

        public decimal Price {
            get => _price;
            set {
                if (_price != value) {
                    _price = value;
                    Notify($"Ціна змінилася на {_price:C}");
                }
            }
        }

        public void Attach(IObserver obs) => _observers.Add(obs);
        public void Notify(string message) {
            foreach (var obs in _observers) obs.Update(message);
        }
    }

    class Program {
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Тест Strategy
            var numbers = new List<int> { 10, 5, 20 };
            var sorter = new Sorter(new QuickSortStrategy());
            sorter.PerformSort(numbers);

            // Тест Observer
            var stock = new StockMonitor();
            stock.Attach(new ConsoleDisplay());
            stock.Price = 2500.50m;
        }
    }
}