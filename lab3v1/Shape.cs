public abstract class Shape
{
    public string Name { get; protected set; }
    public Shape(string name) { Name = name; }
    public abstract double Area();
    public abstract double Perimeter();
    public void DisplayInfo()
    {
        Console.WriteLine($"Фігура: {Name}");
        Console.WriteLine($"Площа: {Area():F2}");
        Console.WriteLine($"Периметр: {Perimeter():F2}");
    }
}