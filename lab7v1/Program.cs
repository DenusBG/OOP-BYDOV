// Program.cs
using System.Net.Http;
using System.IO;
using System;

class Program
{
    static void Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.Title = "Лабораторна Робота №7: Патерн Retry (Варіант 1)";
        Console.WriteLine("=== ЛР7: Патерн Retry, Обробка IO/Мережевих Помилок ===");
        
        var fileProcessor = new FileProcessor();
        var networkClient = new NetworkClient();
        
        // ----------------------------------------------------
        // 1. shouldRetry: Делегат для вибіркової обробки винятків (Варіант 1)
        // ----------------------------------------------------
        Func<Exception, bool> shouldRetryCondition = ex =>
        {
            // Повторюємо спробу лише для FileNotFoundException та HttpRequestException
            return ex is FileNotFoundException || ex is HttpRequestException;
        };

        // ----------------------------------------------------
        // Сценарій 1: FileProcessor (2 невдачі -> Успіх)
        // ----------------------------------------------------
        Console.WriteLine("\n\n####################################################");
        Console.WriteLine("# Сценарій 1: Читання файлу (2 невдачі -> УСПІХ) #");
        Console.WriteLine("####################################################");
        try
        {
            string fileContent = RetryHelper.ExecuteWithRetry(
                () => fileProcessor.ReadFile("settings.conf"), 
                retryCount: 4, 
                initialDelay: TimeSpan.FromSeconds(0.1),
                shouldRetry: shouldRetryCondition
            );
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] Операція з файлом завершена успішно:");
            Console.WriteLine($" -> {fileContent}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[FATAL] Операція з файлом не вдалася. Тип: {ex.GetType().Name}");
        }

        // ----------------------------------------------------
        // Сценарій 2: NetworkClient (3 невдачі -> Успіх)
        // ----------------------------------------------------
        Console.WriteLine("\n\n####################################################");
        Console.WriteLine("# Сценарій 2: Завантаження даних (3 невдачі -> УСПІХ) #");
        Console.WriteLine("####################################################");
        try
        {
            string networkData = RetryHelper.ExecuteWithRetry(
                () => networkClient.DownloadData("https://api.temp.net/data"),
                retryCount: 5, 
                initialDelay: TimeSpan.FromSeconds(0.2), 
                shouldRetry: shouldRetryCondition
            );
            
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("\n[SUCCESS] Операція з мережею завершена успішно:");
            Console.WriteLine($" -> {networkData}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"\n[FATAL] Операція з мережею не вдалася. Тип: {ex.GetType().Name}");
        }
        
        // ----------------------------------------------------
        // Сценарій 3: Демонстрація непідходящого винятку (ArgumentException)
        // ----------------------------------------------------
        Console.WriteLine("\n\n####################################################");
        Console.WriteLine("# Сценарій 3: Непідходящий виняток (STOP IMMEDIATELY) #");
        Console.WriteLine("####################################################");
        try
        {
            RetryHelper.ExecuteWithRetry(
                () => 
                {
                    Console.WriteLine("[Тест] Спроба, що кидає ArgumentException...");
                    throw new ArgumentException("Некоректний аргумент (Не тимчасова помилка).");
                    return "Ніколи не досягається";
                },
                retryCount: 3, 
                initialDelay: TimeSpan.FromSeconds(0.1),
                shouldRetry: shouldRetryCondition
            );
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.WriteLine($"\n[CATCH] Успішно спіймано непідходящий виняток: {ex.GetType().Name}. Повторні спроби були зупинені.");
            Console.ResetColor();
        }
    }
}