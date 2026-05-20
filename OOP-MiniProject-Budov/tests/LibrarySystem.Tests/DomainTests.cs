using LibrarySystem.Domain;
using Xunit;

namespace LibrarySystem.Tests;

public class DomainTests
{
    // Тест 1: Перевірка коректного створення книги
    [Fact]
    public void CreateBook_WithValidData_ShouldInitializeCorrectly()
    {
        var book = new Book("Clean Code", "Robert Martin", "9780132350884");

        Assert.NotNull(book.Id);
        Assert.Equal("Clean Code", book.Title);
        Assert.True(book.IsAvailable);
    }

    // Тест 2: Перевірка інваріанту порожньої назви (очікуємо помилку)
    [Fact]
    public void CreateBook_WithEmptyTitle_ShouldThrowArgumentException()
    {
        Assert.Throws<ArgumentException>(() => new Book("", "Author", "9780132350884"));
    }

    // Тест 3: Успішна видача книги
    [Fact]
    public void Borrow_WhenItemIsAvailable_ShouldSetStatusToUnavailable()
    {
        var book = new Book("C# in Depth", "Jon Skeet", "9781617294532");
        var readerId = Guid.NewGuid();

        book.Borrow(readerId);

        Assert.False(book.IsAvailable);
        Assert.Equal(readerId, book.CurrentReaderId);
    }

    // Тест 4: Спроба взяти вже видану книгу (очікуємо помилку)
    [Fact]
    public void Borrow_WhenItemIsAlreadyBorrowed_ShouldThrowInvalidOperationException()
    {
        var book = new Book("C# in Depth", "Jon Skeet", "9781617294532");
        var reader1 = Guid.NewGuid();
        var reader2 = Guid.NewGuid();

        book.Borrow(reader1);

        Assert.Throws<InvalidOperationException>(() => book.Borrow(reader2));
    }

    // Тест 5: Успішне повернення книги
    [Fact]
    public void ReturnItem_WhenItemIsBorrowed_ShouldMakeItAvailableAgain()
    {
        var book = new Book("C# in Depth", "Jon Skeet", "9781617294532");
        book.Borrow(Guid.NewGuid());

        book.ReturnItem();

        Assert.True(book.IsAvailable);
        Assert.Null(book.CurrentReaderId);
    }
}