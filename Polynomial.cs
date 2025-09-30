using System;
using System.Text;

public class Polynomial
{
    // 1. Приватне поле для зберігання коефіцієнтів многочлена.
    // Це і є ІНКАПСУЛЯЦІЯ: ми ховаємо дані від зовнішнього світу.
    private double[] coefficients;

    // Конструктор класу. Він приймає масив коефіцієнтів.
    // 'params' дозволяє передавати коефіцієнти просто через кому, наприклад: new Polynomial(1, 2, 3)
    public Polynomial(params double[] coeffs)
    {
        // Створюємо копію масиву, щоб уникнути зміни нашого внутрішнього стану ззовні.
        coefficients = new double[coeffs.Length];
        Array.Copy(coeffs, coefficients, coeffs.Length);
    }

    // 2. Індексатор: надає доступ до коефіцієнтів за індексом.
    // Дозволяє працювати з об'єктом класу як зі звичайним масивом (наприклад, poly[0]).
    public double this[int index]
    {
        get
        {
            // Перевірка, чи індекс не виходить за межі масиву
            if (index < 0 || index >= coefficients.Length)
            {
                throw new IndexOutOfRangeException("Індекс коефіцієнта виходить за межі.");
            }
            return coefficients[index];
        }
        set
        {
            // Перевірка, чи індекс не виходить за межі масиву
            if (index < 0 || index >= coefficients.Length)
            {
                throw new IndexOutOfRangeException("Індекс коефіцієнта виходить за межі.");
            }
            coefficients[index] = value;
        }
    }
    
    // Властивість (property), яка повертає степінь многочлена (тільки для читання)
    public int Degree
    {
        get { return coefficients.Length - 1; }
    }

    // 3. Перевантаження оператора додавання (+)
    // 'static' означає, що метод належить класу, а не об'єкту.
    public static Polynomial operator +(Polynomial p1, Polynomial p2)
    {
        // Визначаємо довжину більшого та меншого масивів коефіцієнтів
        int maxDegree = Math.Max(p1.coefficients.Length, p2.coefficients.Length);
        int minDegree = Math.Min(p1.coefficients.Length, p2.coefficients.Length);

        double[] newCoeffs = new double[maxDegree];

        // 1. Додаємо коефіцієнти, які є в обох многочленах
        for (int i = 0; i < minDegree; i++)
        {
            newCoeffs[i] = p1.coefficients[i] + p2.coefficients[i];
        }

        // 2. Копіюємо решту коефіцієнтів з довшого многочлена
        if (p1.coefficients.Length > p2.coefficients.Length)
        {
            for (int i = minDegree; i < maxDegree; i++)
            {
                newCoeffs[i] = p1.coefficients[i];
            }
        }
        else
        {
            for (int i = minDegree; i < maxDegree; i++)
            {
                newCoeffs[i] = p2.coefficients[i];
            }
        }

        // Повертаємо новий об'єкт-многочлен, що є результатом додавання
        return new Polynomial(newCoeffs);
    }
    
    // Перевизначення методу ToString() для красивого виводу многочлена в консоль.
    // Це не обов'язково за завданням, але дуже корисно для перевірки.
    public override string ToString()
    {
        StringBuilder sb = new StringBuilder();
        for (int i = Degree; i >= 0; i--)
        {
            if (coefficients[i] != 0)
            {
                // Додаємо знак + або -
                if (sb.Length > 0)
                {
                    sb.Append(coefficients[i] > 0 ? " + " : " - ");
                }
                else if (coefficients[i] < 0)
                {
                     sb.Append("-");
                }
                
                double absCoeff = Math.Abs(coefficients[i]);

                // Додаємо коефіцієнт (якщо він не 1 або це вільний член)
                if (absCoeff != 1 || i == 0)
                {
                    sb.Append(absCoeff);
                }

                // Додаємо 'x' та степінь
                if (i > 0)
                {
                    sb.Append("x");
                    if (i > 1)
                    {
                        sb.Append($"^{i}");
                    }
                }
            }
        }
        return sb.Length > 0 ? sb.ToString() : "0";
    }
}