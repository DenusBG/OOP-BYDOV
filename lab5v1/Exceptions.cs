// Exceptions.cs
using System;

public class InvalidItemException : Exception
{
    public InvalidItemException() 
        : base("Помилка: Товар має некоректні дані (наприклад, негативну ціну або кількість).") { }

    public InvalidItemException(string message) 
        : base(message) { }

    public InvalidItemException(string message, Exception inner) 
        : base(message, inner) { }
}

public class NotFoundException : Exception
{
    public NotFoundException(string entityName, string id) 
        : base($"Помилка: Об'єкт '{entityName}' з ID {id} не знайдено.") { }
}