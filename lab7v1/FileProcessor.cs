// FileProcessor.cs
using System.IO;
using System;

public class FileProcessor
{
    private int _attempts = 0;
    
    public string ReadFile(string path)
    {
        _attempts++;
        Console.WriteLine($"[FileProcessor] Спроба читання файлу '{path}' (Спроба {_attempts})...");

        // Імітація FileNotFoundException перші 2 рази
        if (_attempts <= 2)
        {
            throw new FileNotFoundException($"Файл '{path}' не знайдено.", path);
        }

        // Успішне виконання на 3-й спробі
        return "Вміст файлу: Це успішні дані, отримані з файлу.";
    }
}