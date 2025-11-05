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
            if (string.IsNullOrWhiteSpace(title)) throw new ArgumentException("Введите название книги");
            if (string.IsNullOrWhiteSpace(author)) throw new ArgumentException("Введитетавтора книги");
            if (year <= 0) throw new ArgumentException("Год должен быть положительным");
            if (price < 0) throw new ArgumentException("Цена не может быть отрицательной");

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
                Console.WriteLine("5. Показать самую дорогую и самую дешевую книгу");
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
                if (!Enum.TryParse(Console.ReadLine(), out Genre genre)) { Console.WriteLine("Неверный жанр."); return; }
                Console.Write("Год: ");
                if (!int.TryParse(Console.ReadLine(), out int year)) { Console.WriteLine("Неверный год."); return; }
                Console.Write("Цена: ");
                if (!decimal.TryParse(Console.ReadLine(), out decimal price)) { Console.WriteLine("Неверная цена."); return; }

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
                    if (!Enum.TryParse(Console.ReadLine(), out Genre genre)) { Console.WriteLine("Неверный жанр."); return; }
                    results = books.Where(b => b.Genre == genre);
                    break;
                default:
                    Console.WriteLine("Неверный вариант."); return;
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

            if (sorted == null) { Console.WriteLine("Неверная команда."); return; }

            Console.WriteLine("\nОтсортированные книги:");
            foreach (var b in sorted) Console.WriteLine(b);
        }
