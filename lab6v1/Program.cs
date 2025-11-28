// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

// 1. Оголошення власного делегата для логічної операції
public delegate bool PriceCheck(decimal price);

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторна Робота №6: Лямбда-вирази та Делегати (Варіант 1) ===");

        // --- Ініціалізація колекції ---
        List<Product> products = new List<Product>
        {
            new Product { Id = 1, Name = "Ноутбук Dell XPS", Price = 35000.00m, Category = "Електроніка" },
            new Product { Id = 2, Name = "Монітор 4K", Price = 12000.00m, Category = "Електроніка" },
            new Product { Id = 3, Name = "Електричний чайник", Price = 850.50m, Category = "Побутова техніка" },
            new Product { Id = 4, Name = "Бездротова миша", Price = 1500.00m, Category = "Аксесуари" },
            new Product { Id = 5, Name = "Смартфон Samsung S21", Price = 25000.00m, Category = "Електроніка" },
            new Product { Id = 6, Name = "Блендер KitchenAid", Price = 4200.00m, Category = "Побутова техніка" }
        };

        // --- 1. Анонімний Метод ---
        Console.WriteLine("\n--- 1. Анонімний Метод (Використання власного делегата) ---");
        // Приклад: використання анонімного методу, присвоєного власному делегату PriceCheck
        PriceCheck checkCheapAnon = delegate(decimal price)
        {
            return price < 5000.00m;
        };
        
        var cheapProductsAnon = products.Where(p => checkCheapAnon(p.Price));
        Console.WriteLine($"Кількість товарів < 5000.00 ₴: {cheapProductsAnon.Count()}");


        // --- 2. Лямбда-вирази та LINQ (Фільтрація за ціною > 10000) ---
        Console.WriteLine("\n--- 2. Лямбда-вирази та LINQ (Where) ---");

        // Приклад: використання лямбда-виразу для фільтрації товарів
        var expensiveProducts = products.Where(p => p.Price > 10000.00m).ToList();
        
        Console.WriteLine("Дорогі товари (> 10 000 ₴):");
        expensiveProducts.ForEach(p => Console.WriteLine($"  {p}")); // Лямбда-вираз для List.ForEach

        
        // --- 3. Вбудовані Делегати ---

        // A) Func<T, TResult> (Обчислення середньої вартості)
        Console.WriteLine("\n--- 3.1. Func<T, TResult> (Обчислення) ---");
        
        // Func<List<Product>, decimal> - Приймає List<Product> і повертає decimal
        Func<List<Product>, decimal> calculateAveragePrice = 
            list => list.Average(p => p.Price); // Лямбда-вираз для Func
            
        decimal avgPrice = calculateAveragePrice(products);
        Console.WriteLine($"Середня вартість усіх товарів: {avgPrice:C2}");


        // B) Predicate<T> (Пошук найдорожчого)
        Console.WriteLine("\n--- 3.2. Predicate<T> (Пошук найдорожчого) ---");
        
        var maxPrice = products.Max(p => p.Price);
        
        // Predicate<Product> - Приймає Product і повертає bool
        Predicate<Product> isMostExpensive = p => p.Price == maxPrice; // Лямбда-вираз для Predicate
        
        // Використовуємо Find (Predicate) для пошуку
        var mostExpensiveProduct = products.Find(isMostExpensive); 
        
        // Виведення результату (mostExpensiveProduct не може бути null, бо колекція не порожня)
        Console.WriteLine($"Найдорожчий товар: {mostExpensiveProduct}");


        // C) Action<T> (Вивід результату)
        Console.WriteLine("\n--- 3.3. Action<T> (Вивід в консоль) ---");

        // Action<string> - Приймає string і не повертає значення
        Action<string> printResult = s => Console.WriteLine($"[INFO] {s}"); // Лямбда-вираз для Action
        
        printResult($"Обчислена середня ціна: {avgPrice:C2}");
        
        
        // --- 4. Додаткові LINQ-операції (Select, OrderBy, Aggregate) ---
        Console.WriteLine("\n--- 4. Додаткові LINQ-операції ---");
        
        // Select (Проєкція: створення анонімного об'єкта)
        var priceList = products
            .Select(p => new { p.Name, PriceWithVAT = p.Price * 1.2m }) 
            .OrderByDescending(p => p.PriceWithVAT); 
            
        Console.WriteLine("Товари з ПДВ (від найдорожчого):");
        priceList.ToList().ForEach(p => Console.WriteLine($"  {p.Name}: {p.PriceWithVAT:C2}"));


        // Aggregate (Обчислення загальної вартості)
        decimal totalValue = products.Aggregate(0m, (currentTotal, product) => currentTotal + product.Price); 
        Console.WriteLine($"\nЗагальна вартість усіх товарів (Aggregate): {totalValue:C2}");
    }
}