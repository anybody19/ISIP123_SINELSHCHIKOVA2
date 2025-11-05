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

