using LibrarySystem.Domain;

namespace LibrarySystem.Infrastructure;

public class InMemoryLibraryRepository : ILibraryRepository
{
    private readonly Dictionary<Guid, LibraryItem> _items = new();
    private readonly Dictionary<Guid, Reader> _readers = new();

    public InMemoryLibraryRepository()
    {
        // Додаємо кілька книг для старту, щоб було що брати
        var book1 = new Book("C# in Depth", "Jon Skeet", "9781617294532");
        var book2 = new Book("Clean Architecture", "Robert C. Martin", "9780134494166");
        
        _items[book1.Id] = book1;
        _items[book2.Id] = book2;
    }

    public LibraryItem GetItemById(Guid id)
    {
        if (!_items.ContainsKey(id))
            throw new KeyNotFoundException($"Ресурс з ID {id} не знайдено.");
        return _items[id];
    }

    public void SaveItem(LibraryItem item)
    {
        _items[item.Id] = item;
    }

    public void AddReader(Reader reader)
    {
        _readers[reader.Id] = reader;
    }

    public Reader GetReaderById(Guid id)
    {
        if (!_readers.ContainsKey(id))
            throw new KeyNotFoundException($"Читача з ID {id} не знайдено.");
        return _readers[id];
    }

    public IEnumerable<LibraryItem> GetAllItems() => _items.Values;
}