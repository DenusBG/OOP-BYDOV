using System;
using System.Collections.Generic;

namespace lab19.Patterns
{
    // --- ПАТЕРН COMPOSITE ---
    public interface IGraphic {
        void Draw();
    }

    public class Circle : IGraphic {
        public void Draw() => Console.WriteLine("Малюємо Коло");
    }

    public class Group : IGraphic {
        private List<IGraphic> _children = new List<IGraphic>();
        public void Add(IGraphic g) => _children.Add(g);
        public void Draw() {
            Console.WriteLine("Група об'єктів:");
            foreach (var item in _children) item.Draw();
        }
    }

    // --- ПАТЕРН DECORATOR ---
    public interface ICoffee {
        string GetDescription();
        double GetCost();
    }

    public class SimpleCoffee : ICoffee {
        public string GetDescription() => "Проста кава";
        public double GetCost() => 40.0;
    }

    public abstract class CoffeeDecorator : ICoffee {
        protected ICoffee _coffee;
        public CoffeeDecorator(ICoffee coffee) => _coffee = coffee;
        public virtual string GetDescription() => _coffee.GetDescription();
        public virtual double GetCost() => _coffee.GetCost();
    }

    public class MilkDecorator : CoffeeDecorator {
        public MilkDecorator(ICoffee coffee) : base(coffee) { }
        public override string GetDescription() => base.GetDescription() + " + Молоко";
        public override double GetCost() => base.GetCost() + 10.0;
    }

    class Program {
        static void Main() {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Тест Composite
            Console.WriteLine("=== ТЕСТ COMPOSITE ===");
            var group = new Group();
            group.Add(new Circle());
            group.Add(new Circle());
            group.Draw();

            // Тест Decorator
            Console.WriteLine("\n=== ТЕСТ DECORATOR ===");
            ICoffee myCoffee = new SimpleCoffee();
            myCoffee = new MilkDecorator(myCoffee);
            Console.WriteLine($"{myCoffee.GetDescription()} Ціна: {myCoffee.GetCost()} грн");
        }
    }
}
