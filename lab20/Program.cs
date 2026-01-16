using System;

namespace lab20
{
    // --- 1. Моделі ---
    public enum OrderStatus { New, Processed }

    public class Order
    {
        public int Id { get; set; }
        public string CustomerName { get; set; }
        public decimal TotalAmount { get; set; }
        public OrderStatus Status { get; set; }

        public Order(int id, string customerName, decimal totalAmount)
        {
            Id = id;
            CustomerName = customerName;
            TotalAmount = totalAmount;
            Status = OrderStatus.New;
        }
    }

    // --- 2. Інтерфейси (SRP) ---
    public interface IOrderValidator { bool IsValid(Order order); }
    public interface IOrderRepository { void Save(Order order); }
    public interface IEmailService { void SendOrderConfirmation(Order order); }

    // --- 3. Реалізації ---
    public class OrderValidator : IOrderValidator 
    { 
        public bool IsValid(Order order) => order.TotalAmount > 0; 
    }

    public class InMemoryOrderRepository : IOrderRepository 
    { 
        public void Save(Order order) => Console.WriteLine($"[БД] Замовлення {order.Id} збережено."); 
    }

    public class ConsoleEmailService : IEmailService 
    { 
        public void SendOrderConfirmation(Order order) => Console.WriteLine($"[Email] Підтвердження відправлено {order.CustomerName}."); 
    }

    // --- 4. Головний сервіс (Координатор) ---
    public class OrderService
    {
        private readonly IOrderValidator _validator;
        private readonly IOrderRepository _repository;
        private readonly IEmailService _emailService;

        public OrderService(IOrderValidator validator, IOrderRepository repository, IEmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void ProcessOrder(Order order)
        {
            Console.WriteLine($"--- Обробка замовлення #{order.Id} ---");
            if (!_validator.IsValid(order))
            {
                Console.WriteLine("Помилка: Невалідна сума замовлення!");
                return;
            }

            _repository.Save(order);
            _emailService.SendOrderConfirmation(order);
            order.Status = OrderStatus.Processed;
            Console.WriteLine("Замовлення успішно оброблено.\n");
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            // Налаштування залежностей
            var service = new OrderService(new OrderValidator(), new InMemoryOrderRepository(), new ConsoleEmailService());

            // Демонстрація
            service.ProcessOrder(new Order(1, "Олексій", 500m));  // Валідне
            service.ProcessOrder(new Order(2, "Андрій", -10m));   // Невалідне
        }
    }
}