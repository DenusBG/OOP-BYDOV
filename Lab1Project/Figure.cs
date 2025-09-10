using System;

namespace Lab1Project
{
    public class Figure
    {
        // Приватне поле
        private double _area;

        // Публічна властивість
        public double Area
        {
            get { return _area; }
            set { _area = value; }
        }

        // Конструктор
        public Figure(double area)
        {
            _area = area;
            Console.WriteLine("Figure created");
        }

        // Деструктор
        ~Figure()
        {
            Console.WriteLine("Figure destroyed");
        }

        // Метод для отримання інформації про фігуру
        public string GetFigure()
        {
            return $"Figure with area: {_area}";
        }
    }
}
