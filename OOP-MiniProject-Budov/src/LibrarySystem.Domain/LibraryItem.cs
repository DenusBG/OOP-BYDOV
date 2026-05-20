namespace LibrarySystem.Domain;

public abstract class LibraryItem
{
    public Guid Id { get; protected set; }
    public string Title { get; protected set; }
    public bool IsAvailable { get; protected set; } = true;
    public Guid? CurrentReaderId { get; protected set; }

    protected LibraryItem(string title)
    {
        if (string.IsNullOrWhiteSpace(title))
            throw new ArgumentException("Назва не може бути порожньою.", nameof(title));
            
        Id = Guid.NewGuid();
        Title = title;
    }

    public virtual void Borrow(Guid readerId)
    {
        if (!IsAvailable)
            throw new InvalidOperationException($"Ресурс '{Title}' вже видано.");

        IsAvailable = false;
        CurrentReaderId = readerId;
    }

    public virtual void ReturnItem()
    {
        IsAvailable = true;
        CurrentReaderId = null;
    }
}