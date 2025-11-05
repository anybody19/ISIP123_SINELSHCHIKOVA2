using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversityManagement
{
    // Абстрактный базовый класс для всех людей в университете
    abstract class Person
    {
        public string Name { get; private set; }
        public int Age { get; private set; }
        public string Email { get; private set; }

        protected Person(string name, int age, string email)
        {
            Name = name;
            Age = age;
            Email = email;
        }

        // Полиморфный метод для отображения информации
        public abstract void ShowInfo();
    }

    class Student : Person
    {
        private List<Course> courses = new List<Course>();

        public Student(string name, int age, string email) : base(name, age, email) { }

        public void EnrollCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.AddStudent(this);
            }
        }

        public void ShowCourses()
        {
            Console.WriteLine($"Курсы студента {Name}:");
            foreach (var course in courses)
            {
                Console.WriteLine($"- {course.Title}");
            }
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Студент: {Name}, Возраст: {Age}, Email: {Email}");
            ShowCourses();
        }
    }

    class Teacher : Person
    {
        private List<Course> courses = new List<Course>();

        public Teacher(string name, int age, string email) : base(name, age, email) { }

        public void AssignCourse(Course course)
        {
            if (!courses.Contains(course))
            {
                courses.Add(course);
                course.SetTeacher(this);
            }
        }

        public void ShowCourses()
        {
            Console.WriteLine($"Преподаваемые курсы {Name}:");
            foreach (var course in courses)
            {
                Console.WriteLine($"- {course.Title}");
            }
        }

        public override void ShowInfo()
        {
            Console.WriteLine($"Преподаватель: {Name}, Возраст: {Age}, Email: {Email}");
            ShowCourses();
        }
    }

    class Course
    {
        public string Title { get; private set; }
        private Teacher teacher;
        private List<Student> students = new List<Student>();

        public Course(string title)
        {
            Title = title;
        }

        public void SetTeacher(Teacher t)
        {
            teacher = t;
        }

        public void AddStudent(Student s)
        {
            if (!students.Contains(s))
                students.Add(s);
        }

        public void ShowInfo()
        {
            Console.WriteLine($"Курс: {Title}");
            Console.WriteLine($"Преподаватель: {(teacher != null ? teacher.Name : "Не назначен")}");
            Console.WriteLine("Студенты:");
            foreach (var s in students)
            {
                Console.WriteLine($"- {s.Name}");
            }
        }
    }

    class University
    {
        private List<Student> students = new List<Student>();
        private List<Teacher> teachers = new List<Teacher>();
        private List<Course> courses = new List<Course>();

        public void AddStudent(Student s) => students.Add(s);
        public void AddTeacher(Teacher t) => teachers.Add(t);
        public void AddCourse(Course c) => courses.Add(c);

        public void ShowAllStudents()
        {
            Console.WriteLine("Все студенты:");
            foreach (var s in students)
                s.ShowInfo();
        }

        public void ShowAllTeachers()
        {
            Console.WriteLine("Все преподаватели:");
            foreach (var t in teachers)
                t.ShowInfo();
        }

        public void ShowAllCourses()
        {
            Console.WriteLine("Все курсы:");
            foreach (var c in courses)
                c.ShowInfo();
        }
        public Student FindStudent(string name) => students.FirstOrDefault(s => s.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        public Teacher FindTeacher(string name) => teachers.FirstOrDefault(t => t.Name.Equals(name, StringComparison.OrdinalIgnoreCase));
        public Course FindCourse(string title) => courses.FirstOrDefault(c => c.Title.Equals(title, StringComparison.OrdinalIgnoreCase));
    }

    class Program
    {
        static void Main(string[] args)
        {
            University uni = new University();

            while (true)
            {
                Console.WriteLine("\n--- Меню Университета ---");
                Console.WriteLine("1. Добавить студента");
                Console.WriteLine("2. Добавить преподавателя");
                Console.WriteLine("3. Создать курс");
                Console.WriteLine("4. Записать студента на курс");
                Console.WriteLine("5. Назначить преподавателя на курс");
                Console.WriteLine("6. Просмотреть все студенты");
                Console.WriteLine("7. Просмотреть всех преподавателей");
                Console.WriteLine("8. Просмотреть все курсы");
                Console.WriteLine("9. Выход");

                Console.Write("Выберите опцию: ");
                string choice = Console.ReadLine();

                switch (choice)
                {
                    case "1":
                        Console.Write("Имя студента: ");
                        string sName = Console.ReadLine();
                        Console.Write("Возраст: ");
                        int sAge = int.Parse(Console.ReadLine());
                        Console.Write("Email: ");
                        string sEmail = Console.ReadLine();
                        uni.AddStudent(new Student(sName, sAge, sEmail));
                        break;

                    case "2":
                        Console.Write("Имя преподавателя: ");
                        string tName = Console.ReadLine();
                        Console.Write("Возраст: ");
                        int tAge = int.Parse(Console.ReadLine());
                        Console.Write("Email: ");
                        string tEmail = Console.ReadLine();
                        uni.AddTeacher(new Teacher(tName, tAge, tEmail));
                        break;

                    case "3":
                        Console.Write("Название курса: ");
                        string cTitle = Console.ReadLine();
                        uni.AddCourse(new Course(cTitle));
                        break;

                    case "4":
                        Console.Write("Имя студента: ");
                        string studentName = Console.ReadLine();
                        Console.Write("Название курса: ");
                        string courseName = Console.ReadLine();
                        var student = uni.FindStudent(studentName);
                        var course = uni.FindCourse(courseName);
                        if (student != null && course != null)
                            student.EnrollCourse(course);
                        else
                            Console.WriteLine("Студент или курс не найден.");
                        break;

                    case "5":
                        Console.Write("Имя преподавателя: ");
                        string teacherName = Console.ReadLine();
                        Console.Write("Название курса: ");
                        string assignCourseName = Console.ReadLine();
                        var teacher = uni.FindTeacher(teacherName);
                        var courseToAssign = uni.FindCourse(assignCourseName);
                        if (teacher != null && courseToAssign != null)
                            teacher.AssignCourse(courseToAssign);
                        else
                            Console.WriteLine("Преподаватель или курс не найден.");
                        break;

                    case "6":
                        uni.ShowAllStudents();
                        break;
                    case "7":
                        uni.ShowAllTeachers();
                        break;

                    case "8":
                        uni.ShowAllCourses();
                        break;

                    case "9":
                        return;

                    default:
                        Console.WriteLine("Неверная опция. Попробуйте снова.");
                        break;
                }
            }
        }
    }
}
