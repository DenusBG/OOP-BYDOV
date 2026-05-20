using LibrarySystem.Application;
using LibrarySystem.Domain;
using LibrarySystem.Infrastructure;

// 1. Налаштування залежностей (Composition Root)
ILibraryRepository repository = new InMemoryLibraryRepository();
LibraryService service = new LibraryService(repository);

Console.WriteLine("=== Вітаємо у системі LibrarySystem ===");

try
{
    // 2. Реєструємо читача
    Console.WriteLine("\nРеєстрація читача...");
    var reader = service.RegisterReader("Олександр Будов");
    Console.WriteLine($"Читача '{reader.Name}' успішно зареєстровано! ID: {reader.Id}");

    // 3. Показуємо доступні книги
    Console.WriteLine("\nДоступні книги в бібліотеці:");
    var allItems = repository.GetAllItems().ToList();
    foreach (var item in allItems)
    {
        Console.WriteLine($"- {item.Title} (ID: {item.Id}) | Доступно: {item.IsAvailable}");
    }

    // 4. Сценарій: Видача книги
    var bookToBorrow = allItems.First(); // Беремо першу-ліпшу книгу для тесту
    Console.WriteLine($"\nСпроба видати книгу '{bookToBorrow.Title}' читачу '{reader.Name}'...");
    
    service.BorrowItem(bookToBorrow.Id, reader.Id);
    Console.WriteLine("Успіх! Книгу видано.");

    // 5. Перевіряємо статус після видачі
    var updatedBook = repository.GetItemById(bookToBorrow.Id);
    Console.WriteLine($"Статус книги '{updatedBook.Title}' після видачі: Доступно = {updatedBook.IsAvailable}");
}
catch (Exception ex)
{
    Console.WriteLine($"\n[ПОМИЛКА] {ex.Message}");
}

Console.ReadLine();