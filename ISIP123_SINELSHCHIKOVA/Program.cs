using System;
using System.Collections.Generic;
using System.Linq;

namespace LibraryApp
{
    public enum Genre
    {
        Fiction,
        NonFiction,
        Mystery,
        Fantasy,
        Science
    }

    public class Book
    {
        private static int nextId = 1;
        public int Id { get; private set; }
        public string Title { get; set; }
        public string Author { get; set; }
        public Genre Genre { get; set; }
        public int Year { get; set; }
        public decimal Price { get; set; }

        public Book(string title, string author, Genre genre, int year, decimal price)
        {
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Название книги не может быть пустым.");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Автор не может быть пустым.");
            if (year <= 0) throw new ArgumentException("Год должен быть положительным.");
            if (price < 0) throw new ArgumentException("Цена не может быть отрицательной.");

            Id = nextId++;
            Title = title;
            Author = author;
            Genre = genre;
            Year = year;
            Price = price;
        }

        public override string ToString()
        {
            return $"ID: {Id}, Название: {Title}, Автор: {Author}, Жанр: {Genre}, Год: {Year}, Цена: {Price:C}";
        }
    }

    class Program
    {
        static List<Book> books = new List<Book>();
        static List<Book> cart = new List<Book>();

        static void Main(string[] args)
        {
            InitializeTestData();

            while (true)
            {
                Console.WriteLine("\n Меню библиотеки ");
                Console.WriteLine("1. Добавить книгу");
                Console.WriteLine("2. Удалить книгу по ID");
                Console.WriteLine("3. Найти книги");
                Console.WriteLine("4. Сортировать книги");
                Console.WriteLine("5. Показать самую дорогую и самую дешевую книги");
                Console.WriteLine("6. Группировка по авторам");
                Console.WriteLine("7. Вставить блок книг");
                Console.WriteLine("8. Добавить книгу в корзину");
                Console.WriteLine("9. Показать корзину и итоговую стоимость");
                Console.WriteLine("0. Выход");
                Console.Write("Выберите команду: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1": AddBook(); break;
                    case "2": DeleteBook(); break;
                    case "3": SearchBooks(); break;
                    case "4": SortBooks(); break;
                    case "5": ShowMostExpensiveAndCheapest(); break;
                    case "6": GroupByAuthors(); break;
                    case "7": BulkImport(); break;
                    case "8": AddToCart(); break;
                    case "9": ShowCart(); break;
                    case "0": return;
                    default: Console.WriteLine("Неверная команда."); break;
                }
            }
        }

        static void InitializeTestData()
        {
            books.Add(new Book("Война и мир", "Лев Толстой", Genre.Fiction, 1869, 1200));
            books.Add(new Book("Мастер и Маргарита", "Михаил Булгаков", Genre.Fiction, 1966, 800));
            books.Add(new Book("Хоббит", "Дж. Р. Р. Толкин", Genre.Fantasy, 1937, 500));
            books.Add(new Book("Атлант расправил плечи", "Айн Рэнд", Genre.NonFiction, 1957, 900));
            books.Add(new Book("Шерлок Холмс", "Артур Конан Дойл", Genre.Mystery, 1892, 450));
        }

        static void AddBook()
        {
            try
            {
                Console.Write("Название: ");
                string title = Console.ReadLine();
                Console.Write("Автор: ");
                string author = Console.ReadLine();
                Console.WriteLine("Жанры: " + string.Join(", ", Enum.GetNames(typeof(Genre))));
                Console.Write("Выберите жанр: ");
                if (!Enum.TryParse(Console.ReadLine(), out Genre genre)) { Console.WriteLine("Неверно указан жанр"); return; }
                Console.Write("Год: ");
                if (!int.TryParse(Console.ReadLine(), out int year)) { Console.WriteLine("Неверно указан год"); return; }
                Console.Write("Цена: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price)) { Console.WriteLine("Неверно указана цена"); return; }

                books.Add(new Book(title, author, genre, year, price));
                Console.WriteLine("Книга добавлена.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Ошибка: {ex.Message}");
            }
        }

        static void DeleteBook()
        {
            Console.Write("Введите ID книги для удаления: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    books.Remove(book);
                    Console.WriteLine("Книга удалена.");
                }
                else Console.WriteLine("Книга не найдена.");
            }
            else Console.WriteLine("Неверный ID.");
        }

        static void SearchBooks()
        {
            Console.WriteLine("Поиск по: 1. Названию 2. Автору 3. Жанру");
            string option = Console.ReadLine();
            IEnumerable<Book> results = new List<Book>();

            switch (option)
            {
                case "1":
                    Console.Write("Введите название: ");
                    string title = Console.ReadLine();
                    results = books.Where(b => b.Title.Contains(title, StringComparison.OrdinalIgnoreCase));
                    break;
                case "2":
                    Console.Write("Введите автора: ");
                    string author = Console.ReadLine();
                    results = books.Where(b => b.Author.Contains(author, StringComparison.OrdinalIgnoreCase));
                    break;
                case "3":
                    Console.WriteLine("Жанры: " + string.Join(", ", Enum.GetNames(typeof(Genre))));
                    Console.Write("Введите жанр: ");
                    if (!Enum.TryParse(Console.ReadLine(), out Genre genre)) { Console.WriteLine("Неверный жанр"); return; }
                    results = books.Where(b => b.Genre == genre);
                    break;
                default:
                    Console.WriteLine("Неверный вариант"); return;
            }

            Console.WriteLine("\nРезультаты поиска:");
            foreach (var b in results) Console.WriteLine(b);
        }

        static void SortBooks()
        {
            Console.WriteLine("Сортировать по: 1. Названию 2. Году");
            string option = Console.ReadLine();
            IEnumerable<Book> sorted = option switch
            {
                "1" => books.OrderBy(b => b.Title),
                "2" => books.OrderBy(b => b.Year),
                _ => null
            };

            if (sorted == null) { Console.WriteLine("Неверная команда"); return; }

            Console.WriteLine("\nОтсортированные книги:");
            foreach (var b in sorted) Console.WriteLine(b);
        }

        static void ShowMostExpensiveAndCheapest()
        {
            if (!books.Any()) { Console.WriteLine("Список книг пуст"); return; }

            var maxPrice = books.Max(b => b.Price);
            var minPrice = books.Min(b => b.Price);

            Console.WriteLine("\nСамая дорогая книга:");
            foreach (var b in books.Where(b => b.Price == maxPrice)) Console.WriteLine(b);
            Console.WriteLine("\nСамая дешёвая книга:");
            foreach (var b in books.Where(b => b.Price == minPrice)) Console.WriteLine(b);
        }

        static void GroupByAuthors()
        {
            var groups = books.GroupBy(b => b.Author);
            Console.WriteLine("\nКоличество книг по авторам:");
            foreach (var g in groups) Console.WriteLine($"{g.Key}: {g.Count()} книг");
        }

        static void BulkImport()
        {
            Console.WriteLine("Вставьте книги в формате: Название;Автор;Жанр;Год;Цена (каждая книга с новой строки). Для завершения оставьте строку пустой.");
            while (true)
            {
                string line = Console.ReadLine();
                if (string.IsNullOrWhiteSpace(line)) break;

                var parts = line.Split(';');
                if (parts.Length != 5) { Console.WriteLine("Неверный формат"); continue; }

                if (!Enum.TryParse(parts[2], out Genre genre)) { Console.WriteLine("Неверный жанр"); continue; }
                if (!int.TryParse(parts[3], out int year)) { Console.WriteLine("Неверный год"); continue; }
                if (!decimal.TryParse(parts[4], out decimal price)) { Console.WriteLine("Неверная цена"); continue; }

                try
                {
                    books.Add(new Book(parts[0], parts[1], genre, year, price));
                }
                catch (Exception ex) { Console.WriteLine($"Ошибка: {ex.Message}"); }
            }
            Console.WriteLine("Блок книг добавлен.");
        }

        static void AddToCart()
        {
            Console.Write("Введите ID книги для добавления в корзину: ");
            if (int.TryParse(Console.ReadLine(), out int id))
            {
                var book = books.FirstOrDefault(b => b.Id == id);
                if (book != null)
                {
                    cart.Add(book);
                    Console.WriteLine("Книга добавлена в корзину.");
                }
                else Console.WriteLine("Книга не найдена.");
            }
            else Console.WriteLine("Неверный ID.");
        }

        static void ShowCart()
        {
            if (!cart.Any()) { Console.WriteLine("Корзина пуста."); return; }

            Console.WriteLine("\nКниги в корзине:");
            foreach (var b in cart) Console.WriteLine(b);

            Console.WriteLine($"Итоговая стоимость: {cart.Sum(b => b.Price):C}");
        }
    }
}


