// Models.cs
using System;
using System.Collections.Generic;
using System.Linq;

// Товарна позиція
public class LineItem
{
    // Виправлено CS8618: додано ?, щоб зробити властивість нульовою
    public string? Product { get; set; } 
    private decimal _price;
    private int _quantity;

    public decimal Price
    {
        get => _price;
        set
        {
            if (value <= 0)
                throw new InvalidItemException($"Ціна товару '{Product}' має бути позитивною. Введено: {value}.");
            _price = value;
        }
    }

    public int Quantity
    {
        get => _quantity;
        set
        {
            if (value <= 0)
                throw new InvalidItemException($"Кількість товару '{Product}' має бути позитивною. Введено: {value}.");
            _quantity = value;
        }
    }

    public decimal Total => Price * Quantity;
}

// Чек
public class Receipt
{
    public Guid Id { get; } = Guid.NewGuid();
    public DateTime Date { get; set; } = DateTime.Now;
    public List<LineItem> Items { get; } = new List<LineItem>();

    public decimal Subtotal => Items.Sum(i => i.Total);
    
    public decimal VATAmount => Subtotal * 0.20m;
    
    public decimal Discount
    {
        get
        {
            if (Subtotal > 2000)
            {
                return Subtotal * 0.05m;
            }
            return 0m;
        }
    }

    public decimal GrandTotal => Subtotal + VATAmount - Discount;

    public override string ToString()
    {
        return $"[Чек ID: {Id.ToString().Substring(0, 8)}] Дата: {Date:dd.MM.yyyy} | Всього: {GrandTotal:C2}";
    }
}