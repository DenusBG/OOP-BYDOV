// Rectangle.cs
public class Rectangle : BaseShape
{
    public double Width { get; set; }
    public double Height { get; set; }

    public Rectangle(double width, double height) : base("Rectangle")
    {
        Width = width;
        Height = height;
    }

    public override double CalculateArea()
    {
        return Width * Height;
    }

    public override string GetParameters()
    {
        return $"Width: {Width:F2}, Height: {Height:F2}";
    }
}