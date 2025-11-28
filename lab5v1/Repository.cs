// Repository.cs
using System;
using System.Collections.Generic;
using System.Linq;

// Узагальнений клас Repository<T>
public class Repository<T> : IRepository<T> where T : class
{
    private readonly Dictionary<Guid, T> _storage = new Dictionary<Guid, T>();

    // Виправлено CS8605, CS8600: явна перевірка null та безпечне приведення типів
    private Guid GetId(T entity)
    {
        var prop = typeof(T).GetProperty("Id");
        
        if (prop == null || prop.PropertyType != typeof(Guid))
            throw new InvalidOperationException($"Сутність {typeof(T).Name} повинна мати властивість Id типу Guid.");

        var idValue = prop.GetValue(entity);
        
        if (idValue == null)
            throw new InvalidOperationException($"Властивість Id сутності {typeof(T).Name} не може бути null.");
            
        return (Guid)idValue;
    }

    public void Add(T entity)
    {
        var id = GetId(entity);
        if (_storage.ContainsKey(id))
            throw new ArgumentException($"Сутність з ID {id} вже існує.");
        _storage.Add(id, entity);
        Console.WriteLine($"[Repo] Додано {typeof(T).Name} з ID: {id.ToString().Substring(0, 8)}");
    }

    public void Remove(Guid id)
    {
        if (!_storage.Remove(id))
            throw new NotFoundException(typeof(T).Name, id.ToString().Substring(0, 8));
        Console.WriteLine($"[Repo] Видалено {typeof(T).Name} з ID: {id.ToString().Substring(0, 8)}");
    }

    // Використовуємо T? для коректної сигнатури (хоча ми кидаємо виняток)
    public T Find(Guid id)
    {
        if (_storage.TryGetValue(id, out T? entity))
            return entity!; // Використовуємо оператор '!', оскільки ми впевнені, що значення не null
        throw new NotFoundException(typeof(T).Name, id.ToString().Substring(0, 8));
    }

    public IEnumerable<T> All() => _storage.Values;

    public IEnumerable<T> Where(Func<T, bool> predicate)
    {
        return _storage.Values.Where(predicate);
    }
}