using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using Polly;

// ====================================================================
// ========================= ЗВІТ (КОМЕНТАРІ) =========================
// ====================================================================

/*
## 📑 Самостійна Робота №11: Кейси Polly/Retry: Короткий Звіт
## Мета: Дослідження та аналіз реальних сценаріїв використання бібліотеки Polly.

### Сценарій 1: Доступ до зовнішнього API, який тимчасово недоступний

**Проблема:** Зовнішній REST API може тимчасово повертати помилки сервера (наприклад, 503 Service Unavailable) через перезавантаження або короткочасні збої. Постійна відмова призведе до збою всієї операції.
**Політика Polly:** **Retry** з експоненційною затримкою (WaitAndRetry).
**Обґрунтування:** Оскільки помилка є тимчасовою, повторні спроби з паузою дають час системі відновитися. Експоненційна затримка (2с, 4с, 8с) запобігає DoS-атаці на перевантажений ресурс.

### Сценарій 2: Захист від критичного збою бази даних

**Проблема:** Якщо база даних (або мікросервіс) зазнає критичного збою (наприклад, повне відключення), постійні запити до нього марнують ресурси застосунку та ще більше навантажують пошкоджений сервіс.
**Політика Polly:** **Circuit Breaker** (Автоматичний вимикач).
**Обґрунтування:** Circuit Breaker зупиняє запити до непрацюючого сервісу після певної кількості послідовних невдач. Це дає сервісу час на відновлення і запобігає каскадним збоям у нашому застосунку.

### Сценарій 3: Захист від "зависаючих" операцій

**Проблема:** Відправка повідомлення до черги або складний звітний запит до DB може іноді "зависнути" (тривати нескінченно довго). Це призводить до блокування потоків та поганого користувацького досвіду.
**Політика Polly:** **Timeout** (Таймаут).
**Обґрунтування:** Використовується, щоб гарантувати, що операція завершиться протягом розумного часу (наприклад, 1.5 секунди). Якщо операція не встигає, вона примусово переривається, що запобігає блокуванню ресурсів.

### Загальні Висновки
Використання Polly дозволяє централізовано керувати відмовостійкістю, перетворюючи "жорсткі" залежності (які викликають збій при найменшій помилці) на "пружні" (які вміють самовідновлюватися). Це критично важливо для сучасних розподілених систем і мікросервісів.
*/

public class Program
{
    private static int _scenario1Attempts = 0;
    private static int _scenario2Failures = 0;
    
    // ====================================================================
    // Імітація: Сценарій 1 (Retry)
    // ====================================================================
    public static string CallExternalApi(string url)
    {
        _scenario1Attempts++;
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ➡️ Attempt {_scenario1Attempts}: Calling API {url}...");

        // Імітуємо 2 послідовні невдачі (тимчасовий збій)
        if (_scenario1Attempts <= 2) 
        {
            throw new HttpRequestException($"API call failed: Status 503 Service Unavailable");
        }
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✅ API call to {url} successful!");
        return "Data from API (Successful)";
    }

    // ====================================================================
    // Імітація: Сценарій 2 (Circuit Breaker)
    // ====================================================================
    public static string CallDatabase()
    {
        _scenario2Failures++;
        Console.Write($"[{DateTime.Now:HH:mm:ss}] ➡️ Calling Database... (Failure Count: {_scenario2Failures})");

        // Імітуємо збій підключення
        if (_scenario2Failures < 6) 
        {
            Console.WriteLine(" ❌");
            throw new InvalidOperationException("Database connection timed out.");
        }
        
        Console.WriteLine(" ✅");
        return "Data from Database";
    }

    // ====================================================================
    // Імітація: Сценарій 3 (Timeout)
    // ====================================================================
    public static string LongRunningOperation()
    {
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ➡️ Starting long operation (Max 1.5s)...");
        
        // Імітація операції, яка триває 3 секунди
        Thread.Sleep(3000); 
        
        Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✅ Operation finished successfully.");
        return "Successful report";
    }

    // ====================================================================
    // ГОЛОВНИЙ МЕТОД: Запуск Сценаріїв
    // ====================================================================
    public static async Task Main(string[] args)
    {
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("==========================================================");
        Console.WriteLine("🚀 Самостійна Робота №11: Демонстрація політик Polly 🚀");
        Console.WriteLine("==========================================================");

        // --- 1. Сценарій: Retry з експоненційною затримкою ---
        Console.WriteLine("\n--- 1. Сценарій: API Call with Retry (WaitAndRetry) ---");
        var retryPolicy = Policy
            .Handle<HttpRequestException>() // Обробляємо мережеві помилки
            .WaitAndRetry(
                3, // Максимум 3 повторні спроби
                retryAttempt => TimeSpan.FromSeconds(Math.Pow(2, retryAttempt)), // Експоненційна затримка: 2с, 4с, 8с
                (exception, timeSpan, retryCount, context) =>
                {
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🔄 Retry {retryCount} after {timeSpan.TotalSeconds:F1}s due to: {exception.Message}");
                    Console.ResetColor();
                }
            );

        try
        {
            string result = retryPolicy.Execute(() => CallExternalApi("https://api.external.com/data"));
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ⭐ Final Result: {result}");
        }
        catch (Exception ex)
        {
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🛑 Operation failed after all retries: {ex.Message}");
        }
        Console.ResetColor();

        
        // --- 2. Сценарій: Circuit Breaker ---
        Console.WriteLine("\n--- 2. Сценарій: Database Call with Circuit Breaker ---");
        
        // Політика: При 3 послідовних збоях, відкрити "вимикач" на 10 секунд
        var circuitBreakerPolicy = Policy
            .Handle<InvalidOperationException>()
            .CircuitBreaker(
                3, // Кількість невдач до відкриття
                TimeSpan.FromSeconds(10), // Час, на який відкривається (пауза)
                onBreak: (ex, breakDelay) => 
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ⚠️ CIRCUIT OPENED! Stopping calls for {breakDelay.TotalSeconds}s due to: {ex.Message}");
                },
                onReset: () => 
                {
                    Console.ForegroundColor = ConsoleColor.Green;
                    Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ✅ CIRCUIT CLOSED! Operation restored.");
                }
            );

        for (int i = 1; i <= 7; i++)
        {
            try
            {
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] Running call {i}...");
                circuitBreakerPolicy.Execute(() => CallDatabase());
            }
            catch (InvalidOperationException) // Спіймати помилку, що призвела до відкриття
            {
                // Не потрібно логувати, оскільки логування збою виконує сама політика CallDatabase
            }
            catch (Polly.CircuitBreaker.BrokenCircuitException ex) // Спіймати, коли вимикач спрацював
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🚫 Call {i} blocked by Circuit Breaker: {ex.Message}");
            }
            Thread.Sleep(500); // Невелика пауза між спробами
            Console.ResetColor();
        }
        

        // --- 3. Сценарій: Timeout ---
        Console.WriteLine("\n--- 3. Сценарій: Long Operation with Timeout ---");
        
        // Політика: Асинхронний таймаут на 1.5 секунди
        var timeoutPolicy = Policy.TimeoutAsync(
            TimeSpan.FromSeconds(1.5), 
            Polly.Timeout.TimeoutStrategy.Pessimistic, // Спроба перервати виконання
            (context, timespan, task) =>
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] ⏱️ TIMEOUT! Operation took too long (> {timespan.TotalSeconds}s). Aborting...");
                Console.ResetColor();
                return Task.CompletedTask;
            }
        );

        try
        {
            // Метод має бути обгорнутий у Func<Task>
            await timeoutPolicy.ExecuteAsync(() => Task.Run(() => LongRunningOperation()));
        }
        catch (Polly.Timeout.TimeoutRejectedException)
        {
            // Цей виняток буде спійманий, якщо операція перевищить ліміт часу
            Console.ForegroundColor = ConsoleColor.Red;
            Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🛑 Operation terminated by Timeout policy.");
        }
        catch (Exception ex)
        {
             Console.ForegroundColor = ConsoleColor.Red;
             Console.WriteLine($"[{DateTime.Now:HH:mm:ss}] 🛑 Unhandled exception: {ex.Message}");
        }
        Console.ResetColor();

        Console.WriteLine("\n==========================================================");
        Console.WriteLine("✅ Демонстрація Polly завершена.");
    }
}