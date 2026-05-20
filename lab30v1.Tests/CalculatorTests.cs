using System;
using Xunit;
using lab30v1;

namespace lab30v1.Tests
{
    public class CalculatorTests
    {
        private readonly Calculator _calculator = new Calculator();

        // --- ТЕСТИ ДЛЯ ДОДАВАННЯ (Add) ---

        [Fact]
        public void Add_SimplePositiveNumbers_ReturnsCorrectSum()
        {
            // Arrange & Act
            double result = _calculator.Add(5, 3);

            // Assert
            Assert.Equal(8, result);
        }

        [Theory]
        [InlineData(10, 5, 15)]
        [InlineData(-5, -5, -10)]
        [InlineData(-3, 7, 4)]
        [InlineData(0, 0, 0)]
        public void Add_MultipleInputs_ReturnsCorrectSum(double a, double b, double expected)
        {
            double result = _calculator.Add(a, b);
            Assert.Equal(expected, result);
        }

        // --- ТЕСТИ ДЛЯ ВІДНІМАННЯ (Subtract) ---

        [Fact]
        public void Subtract_PositiveNumbers_ReturnsCorrectDifference()
        {
            double result = _calculator.Subtract(10, 4);
            Assert.Equal(6, result);
        }

        [Theory]
        [InlineData(0, 5, -5)]
        [InlineData(-5, -10, 5)]
        public void Subtract_MultipleInputs_ReturnsCorrectDifference(double a, double b, double expected)
        {
            double result = _calculator.Subtract(a, b);
            Assert.Equal(expected, result);
        }

        // --- ТЕСТИ ДЛЯ МНОЖЕННЯ (Multiply) ---

        [Fact]
        public void Multiply_ByZero_ReturnsZero()
        {
            double result = _calculator.Multiply(5, 0);
            Assert.Equal(0, result);
        }

        [Theory]
        [InlineData(2, 3, 6)]
        [InlineData(-2, 4, -8)]
        [InlineData(-3, -3, 9)]
        public void Multiply_MultipleInputs_ReturnsCorrectProduct(double a, double b, double expected)
        {
            double result = _calculator.Multiply(a, b);
            Assert.Equal(expected, result);
        }

        // --- ТЕСТИ ДЛЯ ДІЛЕННЯ (Divide) ---

        [Fact]
        public void Divide_ValidInputs_ReturnsCorrectQuotient()
        {
            double result = _calculator.Divide(10, 2);
            Assert.Equal(5, result);
        }

        [Fact]
        public void Divide_ByZero_ThrowsDivideByZeroException()
        {
            // Перевірка викидання помилки (Edge case / Error handling)
            var exception = Assert.Throws<DivideByZeroException>(() => _calculator.Divide(10, 0));
            Assert.Equal("Division by zero is not allowed.", exception.Message);
        }

        // --- ТЕСТИ ДЛЯ ПІДНЕСЕННЯ ДО СТЕПЕНЯ (Power) ---

        [Fact]
        public void Power_PositiveExponent_ReturnsCorrectValue()
        {
            double result = _calculator.Power(2, 3);
            Assert.Equal(8, result);
        }

        [Fact]
        public void Power_ZeroExponent_ReturnsOne()
        {
            double result = _calculator.Power(5, 0);
            Assert.Equal(1, result);
        }

        [Theory]
        [InlineData(2, -1, 0.5)]
        [InlineData(4, -2, 0.0625)]
        public void Power_NegativeExponent_ReturnsCorrectValue(double baseNum, double exp, double expected)
        {
            double result = _calculator.Power(baseNum, exp);
            // Використовуємо перевантаження Assert.Equal з точністю до 4 знаків для double
            Assert.Equal(expected, result, 4);
        }
    }
}
