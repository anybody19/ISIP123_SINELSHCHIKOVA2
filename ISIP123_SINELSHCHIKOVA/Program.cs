using System;
using System.Collections.Generic;

class Program
{
    // Список для хранения статистики 
    static List<string> history = new List<string>();

    // Список союзов, которые не считаются словами
    static string[] stopWords = { "и", "а", "но", "или", "да", "что", "как", "то", "же", "ли", "бы", "у", "в", "на", "с", "к" };

    static void Main()
    {
        while (true)
        {
            Console.WriteLine("Введите текст (не менее 100 символов):");
            string text = Console.ReadLine();

            if (text.Length < 100)
            {
                Console.WriteLine("Текст слишком короткий!\n");
                continue;
            }

            // Вычисляем статистику
            string report = ProcessText(text);
            Console.WriteLine(report);

            // История
            history.Add(report);

            // Предлагаем удалить буквы
            Console.WriteLine("\nХотите удалить некоторые буквы и пересчитать статистику? (y/n)");
            if (Console.ReadLine().ToLower() == "y")
            {
                Console.WriteLine("Введите буквы (без запятых, например: аео):");
                string lettersToRemove = Console.ReadLine().ToLower();

                // Удаление букв
                string newText = RemoveLetters(text, lettersToRemove);

                Console.WriteLine("\nТекст после удаления букв:\n" + newText + "\n");

                string newReport = ProcessText(newText);
                Console.WriteLine(newReport);
                history.Add(newReport);
            }

            // Продолжение или показ текста
            Console.WriteLine("\n1 - Ввести новый текст");
            Console.WriteLine("2 - Показать статистику всех текстов");
            Console.WriteLine("3 - Выход");

            string choice = Console.ReadLine();
            if (choice == "2")
            {
                Console.WriteLine("\n=== История статистики ===\n");
                foreach (var item in history)
                    Console.WriteLine(item + "\n");
            }
            else if (choice == "3")
            {
                break;
            }
        }
    }


    // Основная функция
    static string ProcessText(string text)
    {
        string[] words = SplitWords(text);
        string longest = "";
        string shortest = null;
        int vowelCount = 0, consonantCount = 0;
        int sentenceCount = CountSentences(text);

        // Подсчёт слов без союзов и чисел
        int wordCount = 0;

        // Частота
        int[] freq = new int[65536];

        for (int i = 0; i < words.Length; i++)
        {
            string w = words[i].ToLower();

            // Пропуск пустых
            if (w == "") continue;

            // Пропускаем союзы
            bool isStop = false;
            for (int j = 0; j < stopWords.Length; j++)
                if (w == stopWords[j])
                    isStop = true;
            if (isStop) continue;

            // Пропуск чисел
            bool isNumber = true;
            for (int j = 0; j < w.Length; j++)
                if (!char.IsDigit(w[j]))
                {
                    isNumber = false;
                    break;
                }
            if (isNumber) continue;

            wordCount++;

            // Поиск самого длинного слова
            if (w.Length > longest.Length)
                longest = w;

            // Поиск самого короткого слова
            if (shortest == null || w.Length < shortest.Length)
                shortest = w;
        }

        // Подсчёт гласных / согласных и частотности
        string vowels = "аеёиоуыэюя";
        for (int i = 0; i < text.Length; i++)
        {
            char c = char.ToLower(text[i]);
            if (c >= 'а' && c <= 'я' || c == 'ё')
            {
                if (IsVowel(c))
                    vowelCount++;
                else
                    consonantCount++;

                freq[c]++;
            }
        }
        // отчет
        string report = "Статистика:\n" +
                        $"Количество слов (без союзов и чисел): {wordCount}\n" +
                        $"Самое короткое слово: {shortest}\n" +
                        $"Самое длинное слово: {longest}\n" +
                        $"Количество предложений: {sentenceCount}\n" +
                        $"Гласных: {vowelCount}\n" +
                        $"Согласных: {consonantCount}\n" +
                        $"Частотность букв:\n";

        for (int i = 0; i < freq.Length; i++)
            if (freq[i] > 0)
                report += $"{(char)i}: {freq[i]}\n";

        return report;
    }

    static string[] SplitWords(string text)
    {
        char[] separators = { ' ', ',', '.', '!', '?', ':', ';', '\n', '\t', '"', '—', '-', '(', ')' };
        return text.Split(separators);
    }

    static int CountSentences(string text)
    {
        int count = 0;
        for (int i = 0; i < text.Length; i++)
            if (text[i] == '.' || text[i] == '!' || text[i] == '?')
                count++;
        return count;
    }

    static bool IsVowel(char c)
    {
        string vowels = "аеёиоуыэюя";
        for (int i = 0; i < vowels.Length; i++)
            if (c == vowels[i])
                return true;
        return false;
    }

    static string RemoveLetters(string text, string letters)
    {
        string result = "";
        for (int i = 0; i < text.Length; i++)
        {
            char c = char.ToLower(text[i]);
            bool remove = false;
            for (int j = 0; j < letters.Length; j++)
                if (c == letters[j])
                    remove = true;
            if (!remove)
                result += text[i];
        }
        return result;
    }
}