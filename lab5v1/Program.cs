// Program.cs
using System;
using System.Collections.Generic;
using System.Linq;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("=== Лабораторна Робота №5: Обробка Чеків та Generics ===");
        
        // Створення узагальненого репозиторію
        var receiptRepo = new Repository<Receipt>();

        // --- Демонстрація Генерації Даних та Обробки Винятків ---
        try
        {
            // 1. Створення нормальних чеків
            var receipt1 = CreateReceipt("Чек A: Знижка активна", 
                ("Ноутбук", 1500m, 1), 
                ("Миша", 50m, 2),
                ("Монітор", 600m, 1) // Subtotal = 2250 > 2000 -> Знижка 5%
            );
            receiptRepo.Add(receipt1);

            var receipt2 = CreateReceipt("Чек B: Без знижки",
                ("Клавіатура", 100m, 1),
                ("Кабель", 10m, 5) // Subtotal = 150
            );
            receiptRepo.Add(receipt2);

            // 2. Спроба додати некоректний товар (Валідація та Власний Виняток InvalidItemException)
            Console.WriteLine("\n--- 🛑 Спроба 1: Некоректні дані (Виняток InvalidItemException) ---");
            // Це викличе InvalidItemException, оскільки Price = -10 (в LineItem.set)
            var invalidItem = new LineItem { Product = "Бракований товар", Price = -10m, Quantity = 1 };
        }
        catch (InvalidItemException ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Cпійманий виняток]: {ex.Message}");
            Console.ResetColor();
        }
        catch (Exception ex)
        {
            Console.WriteLine($"[Неочікуваний виняток]: {ex.Message}");
        }

        // --- Демонстрація LINQ та Обчислень ---
        Console.WriteLine("\n--- 📊 Обробка Колекцій (LINQ) ---");
        var allReceipts = receiptRepo.All().ToList();

        decimal totalRevenue = allReceipts.Sum(r => r.GrandTotal);
        Console.WriteLine($"1. Загальна виручка: {totalRevenue:C2}");

        // Обчислення середньої вартості чека (LINQ Average)
        double averageReceiptTotal = allReceipts.Average(r => (double)r.GrandTotal);
        Console.WriteLine($"2. Середня вартість чека: {averageReceiptTotal:C2}");
        
        // Пошук чеків із застосованою знижкою (LINQ Where)
        var discountedReceipts = allReceipts
            .Where(r => r.Discount > 0)
            .ToList();

        Console.WriteLine($"3. Кількість чеків зі знижкою (>2000): {discountedReceipts.Count}");
        
        // Виведення чеків
        Console.WriteLine("\n--- 🧾 Список Всіх Чеков ---");
        foreach (var r in allReceipts)
        {
            Console.WriteLine($"{r}");
            Console.WriteLine($"   > Subtotal: {r.Subtotal:C2} | VAT(20%): {r.VATAmount:C2} | Discount: {r.Discount:C2}");
        }

        // --- Демонстрація Обробки Винятків (NotFoundException) ---
        Console.WriteLine("\n--- 🛑 Спроба 2: Пошук неіснуючого об'єкта (Виняток NotFoundException) ---");
        try
        {
            receiptRepo.Remove(Guid.NewGuid()); // Спроба видалити випадковий ID
        }
        catch (NotFoundException ex) // Обробка власного винятку
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[Cпійманий виняток]: {ex.Message}");
            Console.ResetColor();
        }
    }

    // Допоміжний метод для створення чека
    static Receipt CreateReceipt(string name, params (string Product, decimal Price, int Quantity)[] items)
    {
        var receipt = new Receipt { Date = DateTime.Now };
        Console.WriteLine($"\n[Створення] {name}");

        foreach (var item in items)
        {
            var lineItem = new LineItem { Product = item.Product, Price = item.Price, Quantity = item.Quantity };
            receipt.Items.Add(lineItem);
        }
        return receipt;
    }
}