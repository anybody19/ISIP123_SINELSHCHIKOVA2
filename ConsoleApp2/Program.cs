using System;
using System.Collections.Generic;
using System.Linq;
// Основные сущности: User, Product, Order, PickupPoint, CartItem

// Класс User
// 1. Свойства: Id, Nickname, Password, Cart, OrderHistory
// 2. Методы:
//    - Register(): регистрация нового пользователя с проверкой совпадения пароля
//    - Login(): вход существующего пользователя по никнейму и паролю

// Класс Product
// 1. Свойства: Id, Name, Price, StockQuantity
// 2. Методы:
//    - Display(): вывод информации о продукте на экран

// Класс CartItem
// 1. Свойства: Product, Quantity
// 2. Методы:
//    - AdjustQuantity(): изменить 

// Класс Cart
// 1. Свойства: список CartItem
// 2. Методы:
//    - AddProduct(): добавить продукт в корзину
//    - RemoveProduct(): удалить 
//    - DisplayCart(): показать все товары в корзине
//    - ClearCart(): очистить корзину после покупки

// Класс Order
// 1. Свойства: Id, User, List<CartItem>, DateTime, PickupPoint
// 2. Методы:
//    - DisplayOrder(): показать детали заказа

// Класс PickupPoint
// 1. Свойства: Id, Address
// 2. Методы:
//    - Display(): показать ПВЗ

// Класс Marketplace
// 1. Свойства: списки пользователей, продуктов, ПВЗ, заказов
// 2. Методы:
//    - ShowProducts(): вывод всех продуктов
//    - UserRegister(): процесс регистрации пользователя
//    - UserLogin(): процесс входа пользователя
//    - AddToCart(): добавить продукт в корзину
//    - Checkout(): оформить покупку одного товара или всей корзины
//    - ShowOrderHistory(): показать историю заказов с сортировкой
//    - ShowPickupPoints(): вывод всех ПВЗ

// Основной класс Program
// 1. Запуск консольного меню:
//    - Варианты: Зарегистрироваться, Войти, Просмотр товаров, Выйти
// 2. Обработка действий пользователя:
//    - В зависимости от выбора вызываются методы Marketplace

namespace MarketplaceApp
{
    //Пользователь
    class User
    {
        public int Id { get; set; }
        public string Nickname { get; set; }
        public string Password { get; set; }
        public List<CartItem> Cart { get; set; } = new List<CartItem>();
        public List<Order> OrderHistory { get; set; } = new List<Order>();
    }

    //Товар
    class Product
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int StockQuantity { get; set; }

        public void Display()
        {
            Console.WriteLine($"[{Id}] {Name} - {Price:C} (В наличии: {StockQuantity})");
        }
    }

    //Элемент корзины
    class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }
    }

    //Заказ
    class Order
    {
        public int Id { get; set; }
        public User User { get; set; }
        public List<CartItem> Items { get; set; } = new List<CartItem>();
        public DateTime Date { get; set; }
        public PickupPoint Pickup { get; set; }

        public void DisplayOrder()
        {
            Console.WriteLine($"Заказ #{Id} от {Date}");
            foreach (var item in Items)
            {
                Console.WriteLine($" - {item.Product.Name} x {item.Quantity} = {item.Product.Price * item.Quantity:C}");
            }
            Console.WriteLine($"ПВЗ: {Pickup.Address}\n");
        }
    }

    //ПВЗ
    class PickupPoint
    {
        public int Id { get; set; }
        public string Address { get; set; }
    }

    //Основной
    class Marketplace
    {
        public List<User> Users { get; set; } = new List<User>();
        public List<Product> Products { get; set; } = new List<Product>();
        public List<PickupPoint> PickupPoints { get; set; } = new List<PickupPoint>();
        public List<Order> Orders { get; set; } = new List<Order>();

        private int nextUserId = 1;
        private int nextOrderId = 1;

        //все товары
        public void ShowProducts()
        {
            Console.WriteLine("Список товаров");
            foreach (var product in Products)
            {
                product.Display();
            }
        }

        //Регистрация пользователя
        public User UserRegister()
        {
            Console.Write("Введите имя пользователя: ");
            string nick = Console.ReadLine();
            if (Users.Any(u => u.Nickname == nick))
            {
                Console.WriteLine("Пользователь с таким именем уже существует");
                return null;
            }

            Console.Write("Введите пароль: ");
            string pass1 = Console.ReadLine();
            Console.Write("Повторите пароль: ");
            string pass2 = Console.ReadLine();

            if (pass1 != pass2)
            {
                Console.WriteLine("Пароли не совпадают");
                return null;
            }

            var user = new User { Id = nextUserId++, Nickname = nick, Password = pass1 };
            Users.Add(user);
            Console.WriteLine("Вы зарегестрированы");
            return user;
        }

        //Вход
        public User UserLogin()
        {
            Console.Write("Введите имя пользователя: ");
            string nick = Console.ReadLine();
            Console.Write("Введите пароль: ");
            string pass = Console.ReadLine();

            var user = Users.FirstOrDefault(u => u.Nickname == nick && u.Password == pass);
            if (user == null)
            {
                Console.WriteLine("Неверное имя пользователя или пароль");
            }
            return user;
        }

        //Добавление в корзину
        public void AddToCart(User user)
        {
            ShowProducts();
            Console.Write("Введите ID товара для добавления в корзину: ");
            if (!int.TryParse(Console.ReadLine(), out int productId))
            {
                Console.WriteLine("Неверный ввод");
                return;
            }

            var product = Products.FirstOrDefault(p => p.Id == productId);
            if (product == null || product.StockQuantity <= 0)
            {
                Console.WriteLine("Товар не найден");
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int qty) || qty <= 0 || qty > product.StockQuantity)
            {
                Console.WriteLine("Неверное количество");
                return;
            }

            var cartItem = user.Cart.FirstOrDefault(c => c.Product.Id == product.Id);
            if (cartItem != null)
                cartItem.Quantity += qty;
            else
                user.Cart.Add(new CartItem { Product = product, Quantity = qty });

            Console.WriteLine("Товар добавлен");
        }

        // Содержимое корзины
        public void ShowCart(User user)
        {
            if (!user.Cart.Any())
            {
                Console.WriteLine("В корзине ничего нет");
                return;
            }

            Console.WriteLine("Корзина");
            foreach (var item in user.Cart)
            {
                Console.WriteLine($"{item.Product.Name} x {item.Quantity} = {item.Product.Price * item.Quantity:C}");
            }
        }

        //Покупка
        public void Checkout(User user)
        {
            if (!user.Cart.Any())
            {
                Console.WriteLine("В корзине ничего нет");
                return;
            }

            Console.WriteLine("Выберите ПВЗ:");
            foreach (var pp in PickupPoints)
            {
                Console.WriteLine($"{pp.Id}. {pp.Address}");
            }

            if (!int.TryParse(Console.ReadLine(), out int ppId) || !PickupPoints.Any(p => p.Id == ppId))
            {
                Console.WriteLine("Неверный выбор ПВЗ");
                return;
            }

            var pickup = PickupPoints.First(p => p.Id == ppId);

            var order = new Order
            {
                Id = nextOrderId++,
                User = user,
                Items = new List<CartItem>(user.Cart),
                Date = DateTime.Now,
                Pickup = pickup
            };

            Orders.Add(order);
            user.OrderHistory.Add(order);

            //Списание товара со склада
            foreach (var item in user.Cart)
            {
                item.Product.StockQuantity -= item.Quantity;
            }

            user.Cart.Clear();
            Console.WriteLine("Покупка оформлена");
        }

        //История заказов
        public void ShowOrderHistory(User user)
        {
            if (!user.OrderHistory.Any())
            {
                Console.WriteLine("История заказов пуста");
                return;
            }

            Console.WriteLine("Сортировка: 1 - от новых к старым, 2 - от старых к новым");
            string choice = Console.ReadLine();

            var orders = choice == "1"
                ? user.OrderHistory.OrderByDescending(o => o.Date)
                : user.OrderHistory.OrderBy(o => o.Date);

            foreach (var order in orders)
            {
                order.DisplayOrder();
            }
        }
    }

    class Program
    {
        static void Main(string[] args)
        {
            Marketplace marketplace = new Marketplace();

            //Примеры товаров
            marketplace.Products.Add(new Product { Id = 1, Name = "Ноутбук", Price = 50000, StockQuantity = 10 });
            marketplace.Products.Add(new Product { Id = 2, Name = "Телефон", Price = 20000, StockQuantity = 15 });
            marketplace.Products.Add(new Product { Id = 3, Name = "Наушники", Price = 3000, StockQuantity = 20 });

            //ПВЗ прим
            marketplace.PickupPoints.Add(new PickupPoint { Id = 1, Address = "ул. Ленина, 1" });
            marketplace.PickupPoints.Add(new PickupPoint { Id = 2, Address = "ул. Пушкина, 15" });

            User currentUser = null;

            while (true)
            {
                Console.WriteLine("\nДобро пожаловать в GMWOG");
                Console.WriteLine("1. Зарегистрироваться");
                Console.WriteLine("2. Войти");
                Console.WriteLine("3. Посмотреть товаров");
                Console.WriteLine("4. Добавить в корзину");
                Console.WriteLine("5. Показать корзину");
                Console.WriteLine("6. Оформить покупку");
                Console.WriteLine("7. История заказов");
                Console.WriteLine("0. Выход");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        currentUser = marketplace.UserRegister();
                        break;
                    case "2":
                        currentUser = marketplace.UserLogin();
                        if (currentUser != null) Console.WriteLine($"Добро пожаловать, {currentUser.Nickname}!");
                        break;
                    case "3":
                        marketplace.ShowProducts();
                        break;
                    case "4":
                        if (currentUser == null)
                        {
                            Console.WriteLine("Сначала войдите или зарегистрируйтесь");
                        }
                        else
                        {
                            marketplace.AddToCart(currentUser);
                        }
                        break;
                    case "5":
                        if (currentUser == null)
                        {
                            Console.WriteLine("Сначала войдите или зарегистрируйтесь");
                        }
                        else
                        {
                            marketplace.ShowCart(currentUser);
                        }
                        break;
                    case "6":
                        if (currentUser == null)
                        {
                            Console.WriteLine("Сначала войдите или зарегистрируйтесь");
                        }
                        else
                        {
                            marketplace.Checkout(currentUser);
                        }
                        break;
                    case "7":
                        if (currentUser == null)
                        {
                            Console.WriteLine("Сначала войдите или зарегистрируйтесь");
                        }
                        else
                        {
                            marketplace.ShowOrderHistory(currentUser);
                        }
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Неверный выбор");
                        break;
                }
            }
        }
    }
}
