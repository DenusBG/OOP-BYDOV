using System;

namespace IndependentWork16
{
    public class NewBaseType
    {
        public string ProductName { get; set; }
    }

    // ==========================================
    // Частина 1: Модель даних
    // ==========================================
    public class Order : NewBaseType
    {
        public int Id { get; set; }
        public decimal Amount { get; set; }
    }

    // ==========================================
    // Частина 2: "Поганий" клас (для завдання п.2)
    // (Закоментовано, щоб не заважав роботі правильної версії)
    // ==========================================
    /*
    public class BadOrderProcessor
    {
        public void Process(Order order)
        {
            // Порушення SRP: Усі дії в одному класі
            if (order.Amount < 0)
                throw new Exception("Некоректна сума!");
            
            Console.WriteLine($"Збереження замовлення {order.Id} в БД...");
            Console.WriteLine($"Відправка email про замовлення {order.Id}...");
        }
    }
    */

    // ==========================================
    // Частина 3: Рефакторинг (SRP + DIP)
    // ==========================================

    // 1. Інтерфейси (Абстракції)
    public interface IOrderValidator
    {
        bool Validate(Order order);
    }

    public interface IOrderRepository
    {
        void Save(Order order);
    }

    public interface IEmailService
    {
        void SendConfirmation(Order order);
    }

    // 2. Реалізація інтерфейсів (Конкретні класи)
    public class OrderValidator : IOrderValidator
    {
        public bool Validate(Order order)
        {
            if (order.Amount <= 0)
            {
                Console.WriteLine("Помилка валідації: Сума замовлення повинна бути більше 0.");
                return false;
            }
            if (string.IsNullOrEmpty(order.ProductName))
            {
                Console.WriteLine("Помилка валідації: Назва товару не може бути порожньою.");
                return false;
            }
            Console.WriteLine($"Валідація замовлення {order.Id} пройшла успішно.");
            return true;
        }
    }

    public class OrderRepository : IOrderRepository
    {
        public void Save(Order order)
        {
            // Імітація збереження в БД
            Console.WriteLine($"[БД] Замовлення {order.Id} ({order.ProductName}) збережено в базу даних.");
        }
    }

    public class EmailService : IEmailService
    {
        public void SendConfirmation(Order order)
        {
            // Імітація відправки email
            Console.WriteLine($"[Email] Лист підтвердження для замовлення {order.Id} відправлено клієнту.");
        }
    }

    // 3. Головний сервіс (Orchestrator)
    // Він залежить від інтерфейсів, а не від конкретних класів (DIP)
    public class OrderService
    {
        private readonly IOrderValidator _validator;
        private readonly IOrderRepository _repository;
        private readonly IEmailService _emailService;

        // Constructor Injection
        public OrderService(IOrderValidator validator, IOrderRepository repository, IEmailService emailService)
        {
            _validator = validator;
            _repository = repository;
            _emailService = emailService;
        }

        public void ProcessOrder(Order order)
        {
            Console.WriteLine($"--- Початок обробки замовлення #{order.Id} ---");

            if (!_validator.Validate(order))
            {
                Console.WriteLine("Обробку перервано через помилку валідації.\n");
                return;
            }

            _repository.Save(order);
            _emailService.SendConfirmation(order);

            Console.WriteLine("--- Обробку завершено успішно ---\n");
        }
    }

    // ==========================================
    // Частина 4: Демонстрація (Main)
    // ==========================================
    class Program
    {
        static void Main(string[] args)
        {
            Console.OutputEncoding = System.Text.Encoding.UTF8;

            // Налаштування залежностей (Composition Root)
            IOrderValidator validator = new OrderValidator();
            IOrderRepository repository = new OrderRepository();
            IEmailService emailService = new EmailService();

            // Створення сервісу з впровадженням залежностей
            OrderService orderService = new OrderService(validator, repository, emailService);

            // Тест 1: Валідне замовлення
            Order goodOrder = new Order { Id = 101, ProductName = "Ноутбук", Amount = 25000 };
            orderService.ProcessOrder(goodOrder);

            // Тест 2: Невалідне замовлення (сума від'ємна)
            Order badOrder = new Order { Id = 102, ProductName = "Телефон", Amount = -500 };
            orderService.ProcessOrder(badOrder);
        }
    }
}