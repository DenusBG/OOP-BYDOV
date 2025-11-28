// BaseShape.cs
using System;

public abstract class BaseShape : IArea
{
    public string Name { get; protected set; }

    public BaseShape(string name)
    {
        Name = name;
    }

    public abstract double CalculateArea();

    public abstract string GetParameters();
}