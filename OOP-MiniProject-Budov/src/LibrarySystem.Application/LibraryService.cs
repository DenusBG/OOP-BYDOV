using LibrarySystem.Domain;

namespace LibrarySystem.Application;

public class LibraryService
{
    private readonly ILibraryRepository _repository;

    // Dependency Injection (DI) через конструктор
    public LibraryService(ILibraryRepository repository)
    {
        _repository = repository;
    }

    public Reader RegisterReader(string name)
    {
        var reader = new Reader(name);
        _repository.AddReader(reader);
        return reader;
    }

    public void BorrowItem(Guid itemId, Guid readerId)
    {
        var item = _repository.GetItemById(itemId);
        var reader = _repository.GetReaderById(readerId); // Перевіряємо, чи є читач

        item.Borrow(reader.Id); // Доменна логіка перевірить, чи доступна книга
        _repository.SaveItem(item); // Зберігаємо зміни
    }
}