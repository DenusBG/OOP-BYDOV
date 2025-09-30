using System;
public class Rectangle : Shape
{
    public double Width { get; set; }
    public double Height { get; set; }
    public Rectangle(double width, double height) : base("Прямокутник") { Width = width; Height = height; }
    public override double Area() { return Width * Height; }
    public override double Perimeter() { return 2 * (Width + Height); }
}