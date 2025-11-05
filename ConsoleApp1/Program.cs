using System;
using System.Collections.Generic;
using System.Linq;

namespace StoreInventoryApp
{
    enum Category
    {
        Food = 1,
        Electronics,
        Clothes
    }

    class Product
    {
        private static int nextCode = 1; // Уникальный код будет увеличиваться автоматически

        public int Code { get; private set; }
        public string Name { get; private set; }
        public double Price { get; private set; }
        public int Quantity { get; private set; }
        public bool InStock => Quantity > 0;
        public Category ProductCategory { get; private set; }

        public Product(string name, double price, int quantity, Category category)
        {
            Code = nextCode++;
            Name = name;
            Price = price;
            Quantity = quantity;
            ProductCategory = category;
        }

        public void AddStock(int amount)
        {
            Quantity += amount;
        }

        public bool Sell(int amount)
        {
            if (amount <= Quantity)
            {
                Quantity -= amount;
                return true;
            }
            return false;
        }

        public override string ToString()

        {
            return $"Код: {Code}\nНазвание: {Name}\nЦена: {Price} руб.\nКоличество: {Quantity}\n" +
                   $"В наличии: {(InStock ? "Да" : "Нет")}\nКатегория: {ProductCategory}";
        }
    }

    class Program
    {
        static List<Product> products = new List<Product>();

        static void Main(string[] args)
        {
            // 5 тестовых товаров
            products.Add(new Product("Хлеб", 45, 20, Category.Food));
            products.Add(new Product("Молоко", 80, 15, Category.Food));
            products.Add(new Product("Футболка", 1200, 10, Category.Clothes));
            products.Add(new Product("Телефон", 25000, 5, Category.Electronics));
            products.Add(new Product("Джинсы", 2300, 7, Category.Clothes));

            while (true)
            {
                Console.WriteLine("\n МЕНЮ ");
                Console.WriteLine("1. Добавить товар");
                Console.WriteLine("2. Удалить товар");
                Console.WriteLine("3. Заказать поставку товара");
                Console.WriteLine("4. Продать товар");
                Console.WriteLine("5. Поиск товаров");
                Console.WriteLine("6. Показать все товары");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите действие: ");

                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddProduct(); break;
                    case "2": DeleteProduct(); break;
                    case "3": OrderSupply(); break;
                    case "4": SellProduct(); break;
                    case "5": SearchProducts(); break;
                    case "6": ShowAllProducts(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверный ввод."); break;
                }
            }
        }

        static void AddProduct()
        {
            Console.Write("Введите название: ");
            string name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("Название не может быть пустым.");
                return;
            }

            Console.Write("Введите цену: ");
            if (!double.TryParse(Console.ReadLine(), out double price) || price <= 0)
            {
                Console.WriteLine("Цена должна быть положительным числом.");
                return;
            }

            Console.Write("Введите количество: ");
            if (!int.TryParse(Console.ReadLine(), out int quantity) || quantity < 0)
            {
                Console.WriteLine("Количество не может быть отрицательным.");
                return;
            }
            Console.WriteLine("Выберите категорию: 1 - Food, 2 - Electronics, 3 - Clothes");
            if (!int.TryParse(Console.ReadLine(), out int cat) || cat < 1 || cat > 3)
            {
                Console.WriteLine("Неверный выбор категории.");
                return;
            }

            products.Add(new Product(name, price, quantity, (Category)cat));
            Console.WriteLine("Товар успешно добавлен!");
        }

        static void DeleteProduct()
        {
            Console.Write("Введите код товара для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int code))
            {
                Product p = products.FirstOrDefault(x => x.Code == code);
                if (p != null)
                {
                    products.Remove(p);
                    Console.WriteLine("Товар удалён.");
                }
                else Console.WriteLine("Товар не найден.");
            }
            else Console.WriteLine("Ошибка ввода.");
        }

        static void OrderSupply()
        {
            Console.Write("Введите код товара: ");
            if (int.TryParse(Console.ReadLine(), out int code))
            {
                Product p = products.FirstOrDefault(x => x.Code == code);
                if (p != null)
                {
                    Console.Write("Введите количество для добавления: ");
                    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                    {
                        p.AddStock(amount);
                        Console.WriteLine("Поставка добавлена.");
                    }
                    else Console.WriteLine("Количество должно быть положительным.");
                }
                else Console.WriteLine("Товар не найден.");
            }
        }

        static void SellProduct()
        {
            Console.Write("Введите код товара: ");
            if (int.TryParse(Console.ReadLine(), out int code))
            {
                Product p = products.FirstOrDefault(x => x.Code == code);
                if (p != null)
                {
                    Console.Write("Введите количество для продажи: ");
                    if (int.TryParse(Console.ReadLine(), out int amount) && amount > 0)
                    {
                        if (p.Sell(amount)) Console.WriteLine("Продажа успешна.");
                        else Console.WriteLine("Недостаточно товара на складе.");
                    }
                    else Console.WriteLine("Количество должно быть положительным.");
                }
                else Console.WriteLine("Товар не найден.");
            }
        }


        static void SearchProducts()
        {
            Console.WriteLine("\nПоиск:");
            Console.WriteLine("1. По коду");
            Console.WriteLine("2. По названию");
            Console.WriteLine("3. По категории");
            Console.Write("Выберите: ");

            string choice = Console.ReadLine();

            switch (choice)
            {
                case "1":
                    Console.Write("Введите код: ");
                    if (int.TryParse(Console.ReadLine(), out int code))
                    {
                        var p = products.FirstOrDefault(x => x.Code == code);
                        Console.WriteLine(p != null ? p.ToString() : "Товар не найден.");
                    }
                    break;

                case "2":
                    Console.Write("Введите название: ");
                    string name = Console.ReadLine();
                    var foundByName = products.Where(x => x.Name.Contains(name, StringComparison.OrdinalIgnoreCase));
                    foreach (var item in foundByName) Console.WriteLine("\n" + item);
                    break;

                case "3":
                    Console.WriteLine("Выберите категорию: 1 - Food, 2 - Electronics, 3 - Clothes");
                    if (int.TryParse(Console.ReadLine(), out int cat) && cat >= 1 && cat <= 3)
                    {
                        var foundByCat = products.Where(x => x.ProductCategory == (Category)cat);
                        foreach (var item in foundByCat) Console.WriteLine("\n" + item);
                    }
                    else Console.WriteLine("Неверный ввод");
                    break;

                default:
                    Console.WriteLine("Ошибка выбора.");
                    break;
            }
        }

