// AreaProcessor.cs
using System.Collections.Generic;
using System.Linq;

public class AreaProcessor
{
    private readonly List<IArea> _shapes;

    public AreaProcessor(IEnumerable<IArea> shapes)
    {
        _shapes = new List<IArea>(shapes);
    }

    public double CalculateTotalArea()
    {
        return _shapes.Sum(s => s.CalculateArea());
    }

    public IArea FindMinAreaShape()
    {
        if (!_shapes.Any()) return null;
        return _shapes.OrderBy(s => s.CalculateArea()).FirstOrDefault();
    }

    public IArea FindMaxAreaShape()
    {
        if (!_shapes.Any()) return null;
        return _shapes.OrderByDescending(s => s.CalculateArea()).FirstOrDefault();
    }
}