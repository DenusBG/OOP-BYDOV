using System;

namespace Lab21
{
    // ==========================================
    // 1. Інтерфейс Стратегії (Abstraction)
    // ==========================================
    public interface ITaxiStrategy
    {
        // Розрахунок вартості поїздки
        decimal CalculateCost(decimal distanceKm, decimal idleTimeMinutes);
    }

    // ==========================================
    // 2. Конкретні реалізації (Concrete Strategies)
    // ==========================================

    // Варіант 1: Економ (дешево, без доплат)
    public class EconomyTaxiStrategy : ITaxiStrategy
    {
        public decimal CalculateCost(decimal distanceKm, decimal idleTimeMinutes)
        {
            decimal ratePerKm = 10.0m;
            decimal ratePerMin = 2.0m;
            return (distanceKm * ratePerKm) + (idleTimeMinutes * ratePerMin);
        }
    }

    // Варіант 2: Стандарт (середня ціна + посадка)
    public class StandardTaxiStrategy : ITaxiStrategy
    {
        public decimal CalculateCost(decimal distanceKm, decimal idleTimeMinutes)
        {
            decimal ratePerKm = 15.0m;
            decimal ratePerMin = 3.5m;
            decimal boardingFee = 30.0m; // Плата за посадку
            
            return (distanceKm * ratePerKm) + (idleTimeMinutes * ratePerMin) + boardingFee;
        }
    }

    // Варіант 3: Преміум (висока ціна + податок на розкіш + комфорт)
    public class PremiumTaxiStrategy : ITaxiStrategy
    {
        public decimal CalculateCost(decimal distanceKm, decimal idleTimeMinutes)
        {
            decimal ratePerKm = 25.0m;
            decimal ratePerMin = 10.0m;
            decimal serviceFee = 100.0m; // Фіксована преміум надбавка
            
            return (distanceKm * ratePerKm) + (idleTimeMinutes * ratePerMin) + serviceFee;
        }
    }

    // ==========================================
    // ДЕМОНСТРАЦІЯ OCP: Нова стратегія
    // Ми додали цей клас, не змінюючи інтерфейс
    // або логіку інших стратегій.
    // ==========================================
    public class NightTaxiStrategy : ITaxiStrategy
    {
        public decimal CalculateCost(decimal distanceKm, decimal idleTimeMinutes)
        {
            // Нічний тариф: Стандарт * 1.5
            decimal baseCost = (distanceKm * 15.0m) + (idleTimeMinutes * 3.5m) + 30.0m;
            return baseCost * 1.5m; 
        }
    }

    // ==========================================
    // 3. Фабрика (Factory Method)
    // ==========================================
    public static class TaxiStrategyFactory
    {
        public static ITaxiStrategy CreateStrategy(string type)
        {
            // Приводимо до нижнього регістру для зручності
            switch (type.ToLower())
            {
                case "economy":
                    return new EconomyTaxiStrategy();
                case "standard":
                    return new StandardTaxiStrategy();
                case "premium":
                    return new PremiumTaxiStrategy();
                // Додаємо нову стратегію у фабрику
                case "night":
                    return new NightTaxiStrategy();
                default:
                    throw new ArgumentException("Невідомий тип таксі");
            }
        }
    }

    // ==========================================
    // 4. Контекст (Service)
    // Цей клас дотримується OCP: йому все одно, 
    // яку саме стратегію йому передали.
    // ==========================================
    public class TaxiService
    {
        public decimal CalculateRideCost(decimal distance, decimal idleTime, ITaxiStrategy strategy)
        {
            if (strategy == null)
            {
                throw new ArgumentNullException(nameof(strategy));
            }

            // Делегування розрахунку конкретній стратегії
            return strategy.CalculateCost(distance, idleTime);
        }
    }

    // ==========================================
    // 5. Головний метод (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;
            TaxiService taxiService = new TaxiService();

            Console.WriteLine("=== Калькулятор вартості таксі (Lab 21) ===");

            try
            {
                // 1. Введення даних
                Console.Write("Введіть відстань (км): ");
                decimal distance = decimal.Parse(Console.ReadLine());

                Console.Write("Введіть час простою (хв): ");
                decimal idleTime = decimal.Parse(Console.ReadLine());

                // 2. Вибір стратегії
                Console.WriteLine("\nОберіть тариф:");
                Console.WriteLine("- Economy");
                Console.WriteLine("- Standard");
                Console.WriteLine("- Premium");
                Console.WriteLine("- Night (Новий!)");
                
                Console.Write("Ваш вибір: ");
                string type = Console.ReadLine();

                // 3. Використання фабрики
                ITaxiStrategy strategy = TaxiStrategyFactory.CreateStrategy(type);

                // 4. Розрахунок через сервіс
                decimal cost = taxiService.CalculateRideCost(distance, idleTime, strategy);

                // 5. Виведення результату
                Console.WriteLine("--------------------------------");
                Console.WriteLine($"Обраний тариф: {strategy.GetType().Name}");
                Console.WriteLine($"Всього до сплати: {cost:C2} (грн)");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"\nПомилка: {ex.Message}");
            }

            Console.ReadKey();
        }
    }
}