// NetworkClient.cs
using System.Net.Http;
using System;

public class NetworkClient
{
    private int _attempts = 0;

    public string DownloadData(string url)
    {
        _attempts++;
        Console.WriteLine($"[NetworkClient] Спроба завантаження з '{url}' (Спроба {_attempts})...");

        // Імітація HttpRequestException перші 3 рази
        if (_attempts <= 3)
        {
            throw new HttpRequestException($"Помилка мережі при доступі до {url}. Не вдалося встановити з'єднання.");
        }

        // Успішне виконання на 4-й спробі
        return "Дані з мережі: Успішно завантажено JSON-відповідь від сервера.";
    }
}