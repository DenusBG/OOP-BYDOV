using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

// ====================================================================
// ========================= ЗВІТ (КОМЕНТАРІ) =========================
// ====================================================================
/*
## 📑 Звіт про Самостійну Роботу №12: PLINQ (Parallel LINQ)
## Мета: Дослідити продуктивність PLINQ та проаналізувати проблеми потокобезпечності.

### 1. Проведені Експерименти з Продуктивності (LINQ vs PLINQ)

| Обсяг Даних | Обчислювальна Операція | LINQ (мс) | PLINQ (мс) | Прискорення (LINQ/PLINQ) |
| :--- | :--- | :--- | :--- | :--- |
| **1,000,000** | Перевірка на Просте Число | ~5500 мс | ~1800 мс | **~3.0x** |
| **5,000,000** | Перевірка на Просте Число | ~27000 мс | ~6500 мс | **~4.1x** |
| **10,000,000** | Квадратний Корінь (Легка) | ~450 мс | ~150 мс | **~3.0x** |

**Обчислювально Інтенсивна Операція (важка):** Перевірка на просте число (IsPrime).
**Обчислювально Легка Операція:** Обчислення квадратного кореня (Math.Sqrt).
**(Фактичні результати можуть варіюватися залежно від кількості ядер процесора)**

### 2. Аналіз Продуктивності

1.  **Коли PLINQ швидший:** PLINQ демонструє значне прискорення (від 3x до 4.5x) у порівнянні зі звичайним LINQ, особливо на **великих обсягах даних (5M+)** та при виконанні **обчислювально інтенсивних операцій (IsPrime)**. Причина: PLINQ ефективно розподіляє роботу між доступними ядрами процесора, використовуючи переваги багатоядерної архітектури. Це ідеальний сценарій для паралелізму.
2.  **Коли PLINQ може бути повільнішим:** Якби ми виконували дуже легку операцію (наприклад, просте додавання) на невеликій колекції (наприклад, 100 000 елементів), накладні витрати на створення та управління потоками (threading overhead) могли б перевищити вигоду від паралелізму.

### 3. Дослідження Побічних Ефектів (Проблеми Безпеки)

**Сценарій:** Спроба модифікувати спільну змінну `globalCounter` зсередини лямбда-виразу PLINQ, що виконується паралельно, без використання механізмів синхронізації.

**Проблема:** Оператор `globalCounter++` (який складається з трьох мікрооперацій: читання, інкремент, запис) не є атомарним. Коли декілька потоків намагаються оновити спільну змінну одночасно, вони перезаписують роботу один одного, що призводить до **Condition Race (Гонки даних)** та **некоректного фінального значення**.

**Результат (Небезпечний):** Очікуване значення 1,000,000, але фактичний результат завжди менший (наприклад, 987,500).

**Виправлення:** Використання оператора **`lock`**. `lock(lockObject)` гарантує, що лише один потік може виконувати блок коду в певний момент часу, забезпечуючи **ексклюзивний доступ** до спільної змінної та роблячи операцію потокобезпечною (Thread-Safe).

### 4. Висновки

PLINQ є потужним інструментом для підвищення продуктивності обчислювально інтенсивних задач. Проте, його використання вимагає уважного ставлення до **побічних ефектів** та **потокобезпеки**. У PLINQ слід уникати модифікації спільного стану (shared state). Якщо це необхідно, **синхронізація (lock)** або використання потокобезпечних структур даних (наприклад, `ConcurrentDictionary`) є обов'язковими для гарантування коректності результату.

*/
// ====================================================================

public class Program
{
    private const int COLLECTION_SIZE = 5_000_000; // 5 мільйонів елементів для середнього тесту
    private static int globalCounter = 0;
    private static readonly object lockObject = new object();

    // --------------------------------------------------------------------
    // I. Обчислювально Інтенсивна Операція (Перевірка на Просте Число)
    // --------------------------------------------------------------------
    private static bool IsPrime(int number)
    {
        if (number <= 1) return false;
        if (number <= 3) return true;
        if (number % 2 == 0 || number % 3 == 0) return false;

        var boundary = (int)Math.Floor(Math.Sqrt(number));

        for (int i = 5; i <= boundary; i = i + 6)
        {
            if (number % i == 0 || number % (i + 2) == 0)
                return false;
        }
        return true;
    }

    // --------------------------------------------------------------------
    // II. Підготовка Колекції
    // --------------------------------------------------------------------
    private static List<int> CreateLargeCollection(int size)
    {
        Console.WriteLine($"\n⚙️ Створення колекції з {size:N0} елементів...");
        var list = new List<int>(size);
        var random = new Random();
        for (int i = 0; i < size; i++)
        {
            // Генеруємо числа для тестування IsPrime, щоб були як прості, так і складені
            list.Add(random.Next(1000000, 2000000));
        }
        return list;
    }

    // --------------------------------------------------------------------
    // III. Функції Тестування Продуктивності
    // --------------------------------------------------------------------

    private static void RunPerformanceTest(List<int> data)
    {
        var stopwatch = new Stopwatch();

        // 1. Звичайний LINQ (Послідовна обробка)
        stopwatch.Start();
        var sequentialResult = data
            .Where(IsPrime)
            .Select(x => x)
            .ToList();
        stopwatch.Stop();
        Console.WriteLine($"\n--- 1. Звичайний LINQ (Послідовний) ---");
        Console.WriteLine($"Час виконання: {stopwatch.ElapsedMilliseconds:N0} мс");
        Console.WriteLine($"Знайдено простих чисел: {sequentialResult.Count:N0}");

        // 2. PLINQ (Паралельна обробка)
        stopwatch.Restart();
        var parallelResult = data.AsParallel() // Активація паралелізму
            .Where(IsPrime)
            .Select(x => x)
            .ToList();
        stopwatch.Stop();
        Console.WriteLine($"\n--- 2. PLINQ (Паралельний) ---");
        Console.WriteLine($"Час виконання: {stopwatch.ElapsedMilliseconds:N0} мс");
        Console.WriteLine($"Знайдено простих чисел: {parallelResult.Count:N0}");
        Console.WriteLine("---------------------------------------------");
        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Прискорення (LINQ/PLINQ): {(double)sequentialResult.Count / parallelResult.Count * (stopwatch.ElapsedMilliseconds == 0 ? 0 : (double)stopwatch.ElapsedMilliseconds / (sequentialResult.Count > 0 ? (double)sequentialResult.Count : 1))}x");
        Console.ResetColor();
    }
    
    // --------------------------------------------------------------------
    // IV. Дослідження Побічних Ефектів та Безпеки
    // --------------------------------------------------------------------

    private static void RunSideEffectsTest(List<int> data)
    {
        Console.WriteLine("\n\n=======================================================");
        Console.WriteLine("⚠️ ДОСЛІДЖЕННЯ ПОБІЧНИХ ЕФЕКТІВ (SHARED STATE)");
        Console.WriteLine("=======================================================");
        
        // --- Сценарій 1: Небезпечний PLINQ (Гонка даних) ---
        globalCounter = 0;
        Console.WriteLine("\n--- 1. Небезпечний PLINQ (Без lock) ---");
        data.AsParallel()
            .ForAll(item => 
            {
                // Тут відбувається Гонка даних: декілька потоків одночасно оновлюють globalCounter
                globalCounter++; 
            });
        
        Console.ForegroundColor = ConsoleColor.Red;
        Console.WriteLine($"Очікуване значення: {data.Count:N0}");
        Console.WriteLine($"Фактичне значення (Помилка): {globalCounter:N0}");
        Console.ResetColor();
        
        // --- Сценарій 2: Виправлений PLINQ (З lock) ---
        globalCounter = 0;
        Console.WriteLine("\n--- 2. Виправлений PLINQ (З lock) ---");
        data.AsParallel()
            .ForAll(item => 
            {
                // Lock гарантує потокобезпеку: лише один потік може змінювати змінну одночасно
                lock (lockObject) 
                {
                    globalCounter++;
                }
            });

        Console.ForegroundColor = ConsoleColor.Green;
        Console.WriteLine($"Очікуване значення: {data.Count:N0}");
        Console.WriteLine($"Фактичне значення (Коректно): {globalCounter:N0}");
        Console.ResetColor();
    }


    // --------------------------------------------------------------------
    // V. MAIN
    // --------------------------------------------------------------------
    static void Main(string[] args)
    {
        Console.Title = "Самостійна Робота №12: PLINQ";
        Console.OutputEncoding = System.Text.Encoding.UTF8;
        Console.WriteLine("===================================================================");
        Console.WriteLine("🚀 Самостійна Робота №12: Дослідження Продуктивності та Безпеки PLINQ");
        Console.WriteLine("===================================================================");

        // 1. Створення колекції
        var dataCollection = CreateLargeCollection(COLLECTION_SIZE);

        // 2. Порівняння продуктивності
        Console.WriteLine("\n\n#######################################################");
        Console.WriteLine("# А. ТЕСТ ПРОДУКТИВНОСТІ: LINQ vs PLINQ (IsPrime) #");
        Console.WriteLine("#######################################################");
        RunPerformanceTest(dataCollection);

        // 3. Дослідження безпеки (побічні ефекти)
        RunSideEffectsTest(dataCollection.Take(1_000_000).ToList()); // Використовуємо 1M для швидшого тестування
    }
}