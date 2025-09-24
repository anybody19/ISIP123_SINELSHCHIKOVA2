using System;

class Program
{
    static void Main()
    {
        Console.WriteLine("Введите количество операций (от 2 до 40):");
        int n = int.Parse(Console.ReadLine());

        string[] names = new string[n];
        double[] amounts = new double[n];

        for (int i = 0; i < n; i++)
        {
            Console.WriteLine($"Введите операцию {i + 1} по шаблону (Название услуги/товара; цена):");
            string input = Console.ReadLine();
            string[] parts = input.Split(';');
            names[i] = parts[0].Trim();
            amounts[i] = double.Parse(parts[1].Trim());
        }

        while (true)
        {
            Console.WriteLine("\nМеню:");
            Console.WriteLine("1. Вывод данных");
            Console.WriteLine("2. Статистика (среднее, максимальное, минимальное, сумма)");
            Console.WriteLine("3. Сортировка по цене (пузырьковая сортировка)");
            Console.WriteLine("4. Конвертация валюты");
            Console.WriteLine("5. Поиск по названию");
            Console.WriteLine("0. Выход");
            Console.Write("Выберите пункт меню: ");
            string choice = Console.ReadLine();

            if (choice == "0")
                break;

            switch (choice)
            {
                case "1":
                    PrintData(names, amounts);
                    break;
                case "2":
                    PrintStatistics(amounts);
                    break;
                case "3":
                    BubbleSort(names, amounts);
                    Console.WriteLine("Данные отсортированы по цене.");
                    break;
                case "4":
                    ConvertCurrency(amounts);
                    break;
                case "5":
                    SearchByName(names, amounts);
                    break;
                default:
                    Console.WriteLine("Неверный выбор.");
                    break;
            }
        }
    }

    static void PrintData(string[] names, double[] amounts)
    {
        Console.WriteLine("\nСписок трат:");
        for (int i = 0; i < names.Length; i++)
        {
            Console.WriteLine($"{names[i]} - {amounts[i]:F2} руб.");
        }
    }

    static void PrintStatistics(double[] amounts)
    {
        double sum = 0;
        double max = amounts[0];
        double min = amounts[0];
        for (int i = 0; i < amounts.Length; i++)
        {
            sum += amounts[i];
            if (amounts[i] > max) max = amounts[i];
            if (amounts[i] < min) min = amounts[i];
        }
        double avg = sum / amounts.Length;
        Console.WriteLine($"\nСтатистика:");
        Console.WriteLine($"Сумма: {sum:F2} руб.");
        Console.WriteLine($"Среднее: {avg:F2} руб.");
        Console.WriteLine($"Максимальное: {max:F2} руб.");
        Console.WriteLine($"Минимальное: {min:F2} руб.");
    }

    static void BubbleSort(string[] names, double[] amounts)
    {
        int n = amounts.Length;
        for (int i = 0; i < n - 1; i++)
        {
            int minIndex = i;
            for (int j = i + 1; j < n; j++)
            {
                if (amounts[j] < amounts[minIndex])
                    minIndex = j;
            }
            if (minIndex != i)
            {
                // Меняем местами суммы
                double tempAmount = amounts[i];
                amounts[i] = amounts[minIndex];
                amounts[minIndex] = tempAmount;

                // Меняем местами названия
                string tempName = names[i];
                names[i] = names[minIndex];
                names[minIndex] = tempName;
            }
        }
    }

    static void ConvertCurrency(double[] amounts)
    {
        Console.WriteLine("\nВыберите курс конвертации:");
        Console.WriteLine("1. Доллар США (примерно 90 руб.)");
        Console.WriteLine("2. Евро (примерно 100 руб.)");
        Console.WriteLine("3. Ввести свой курс");
        Console.Write("Ваш выбор: ");
        string choice = Console.ReadLine();
        double rate = 1;

        switch (choice)
        {
            case "1":
                rate = 90;
                break;
            case "2":
                rate = 100;
                break;
            case "3":
                Console.Write("Введите курс конвертации (сколько рублей в одной единице валюты): ");
                if (!double.TryParse(Console.ReadLine(), out rate) || rate <= 0)
                {
                    Console.WriteLine("Некорректный ввод курса. Конвертация отменена.");
                    return;
                }
                break;
            default:
                Console.WriteLine("Неверный выбор, конвертация отменена.");
                return;
        }

        Console.WriteLine("\nВыберите направление конвертации:");
        Console.WriteLine("1. Из рублей в выбранную валюту");
        Console.WriteLine("2. Из выбранной валюты в рубли");
        Console.Write("Ваш выбор: ");
        string direction = Console.ReadLine();

        if (direction == "1")
        {
            for (int i = 0; i < amounts.Length; i++)
                amounts[i] /= rate;

            double total = 0;
            foreach (var amount in amounts)
                total += amount;

            Console.WriteLine($"\nДанные конвертированы из рублей в валюту по курсу {rate:F2}.");
            Console.WriteLine($"Сумма всех покупок в выбранной валюте: {total:F2}");
        }
        else if (direction == "2")
        {
            for (int i = 0; i < amounts.Length; i++)
                amounts[i] *= rate;

            double total = 0;
            foreach (var amount in amounts)
                total += amount;

            Console.WriteLine($"\nДанные конвертированы из валюты в рубли по курсу {rate:F2}.");
            Console.WriteLine($"Сумма всех покупок в рублях: {total:F2}");
        }
        else
        {
            Console.WriteLine("Неверный выбор направления, конвертация отменена.");
        }
    }

    static void SearchByName(string[] names, double[] amounts)
    {
        Console.Write("\nВведите название для поиска: ");
        string search = Console.ReadLine().ToLower();

        bool found = false;
        for (int i = 0; i < names.Length; i++)
        {
            if (names[i].ToLower().Contains(search))
            {
                Console.WriteLine($"{names[i]} - {amounts[i]:F2} руб.");
                found = true;
            }
        }

        if (!found)
            Console.WriteLine("Совпадений не найдено."); 
    }
}
