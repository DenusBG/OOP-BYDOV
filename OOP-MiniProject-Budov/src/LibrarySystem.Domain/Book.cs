namespace LibrarySystem.Domain;

public class Book : LibraryItem
{
    public string Author { get; private set; }
    public string Isbn { get; private set; }

    public Book(string title, string author, string isbn) : base(title)
    {
        if (string.IsNullOrWhiteSpace(author))
            throw new ArgumentException("Автор не може бути порожнім.", nameof(author));
        
        // Перевірка інваріанту в конструкторі
        if (isbn.Length < 10)
            throw new ArgumentException("Некоректний формат ISBN.", nameof(isbn));

        Author = author;
        Isbn = isbn;
    }
}