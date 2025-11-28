// Product.cs

using System;

public class Product
{
    // Ініціалізація рядкових властивостей порожніми рядками для уникнення CS8618
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public decimal Price { get; set; }
    public string Category { get; set; } = string.Empty;

    public override string ToString()
    {
        return $"[{Id}] {Name} ({Category}) - {Price:C2}";
    }
}