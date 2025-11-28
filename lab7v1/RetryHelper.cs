// RetryHelper.cs
using System;
using System.Threading;

public static class RetryHelper
{
    public static T ExecuteWithRetry<T>(
        Func<T> operation, 
        int retryCount = 3, 
        TimeSpan initialDelay = default, 
        Func<Exception, bool>? shouldRetry = null)
    {
        // Встановлення початкової затримки
        if (initialDelay == default) initialDelay = TimeSpan.FromSeconds(0.2);

        for (int attempt = 1; attempt <= retryCount; attempt++)
        {
            try
            {
                // 1. Спроба виконати операцію
                Console.ForegroundColor = ConsoleColor.Green;
                Console.WriteLine($"\n--> Виконання операції (Спроба {attempt}/{retryCount})...");
                Console.ResetColor();
                
                return operation();
            }
            catch (Exception ex)
            {
                // 2. Перевірка, чи це була остання спроба
                if (attempt == retryCount)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[FATAL] Операція не вдалася після {retryCount} спроб. Причина: {ex.GetType().Name}.");
                    Console.ResetColor();
                    throw; 
                }

                // 3. Вибіркове повторення спроби
                if (shouldRetry != null && !shouldRetry(ex))
                {
                    Console.ForegroundColor = ConsoleColor.Magenta;
                    Console.WriteLine($"[ERROR] Виняток {ex.GetType().Name} не підлягає повторній спробі. Припинення.");
                    Console.ResetColor();
                    throw;
                }

                // 4. Логування невдачі та розрахунок затримки
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine($"[WARN] Помилка: {ex.GetType().Name}. Повторна спроба через затримку...");

                // Експоненційна затримка: initialDelay * 2^(attempt-1)
                TimeSpan delay = TimeSpan.FromMilliseconds(initialDelay.TotalMilliseconds * Math.Pow(2, attempt - 1));
                Console.WriteLine($"[DELAY] Очікування: {delay.TotalSeconds:F2} сек.");
                Console.ResetColor();

                Thread.Sleep(delay);
            }
        }
        throw new InvalidOperationException("Помилка в логіці RetryHelper.");
    }
}