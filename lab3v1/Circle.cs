using System;
public class Circle : Shape
{
    public double Radius { get; set; }
    public Circle(double radius) : base("Коло") { Radius = radius; }
    public override double Area() { return Math.PI * Radius * Radius; }
    public override double Perimeter() { return 2 * Math.PI * Radius; }
}