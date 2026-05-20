namespace LibrarySystem.Domain;

public class Reader
{
    public Guid Id { get; private set; }
    public string Name { get; private set; }

    public Reader(string name)
    {
        if (string.IsNullOrWhiteSpace(name))
            throw new ArgumentException("Ім'я читача не може бути порожнім.", nameof(name));

        Id = Guid.NewGuid();
        Name = name;
    }
}