// Circle.cs
using System;

public class Circle : BaseShape // <-- Виправлення
{
    public double Radius { get; set; }

    public Circle(double radius) : base("Circle")
    {
        Radius = radius;
    }

    public override double CalculateArea()
    {
        return Math.PI * Radius * Radius;
    }

    public override string GetParameters()
    {
        return $"Radius: {Radius:F2}";
    }
}