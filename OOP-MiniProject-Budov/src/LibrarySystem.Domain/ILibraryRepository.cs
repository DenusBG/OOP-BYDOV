namespace LibrarySystem.Domain;

public interface ILibraryRepository
{
    LibraryItem GetItemById(Guid id);
    void SaveItem(LibraryItem item);
    void AddReader(Reader reader);
    Reader GetReaderById(Guid id);
    IEnumerable<LibraryItem> GetAllItems();
}