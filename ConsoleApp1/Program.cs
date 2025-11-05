using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversitySystem
{
    //Студент
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }

    //Преподавател
    class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }

    //Курс
    class Course
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public Teacher Teacher { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();

        public void EnrollStudent(Student student)
        {
            if (!Students.Contains(student))
            {
                Students.Add(student);
                student.Courses.Add(this);
                Console.WriteLine($"Студент {student.Name} записан на курс {Title}.");
            }
            else
            {
                Console.WriteLine("Студент уже записан на курс.");
            }
        }

        // Менеджер системы
    class UniversityManager
        {
            public List<Student> Students = new List<Student>();
            public List<Teacher> Teachers = new List<Teacher>();
            public List<Course> Courses = new List<Course>();

            public void CreateStudent()
            {
                Console.Write("Введите имя студента: ");
                string name = Console.ReadLine();

                Console.Write("Введите email: ");
                string email = Console.ReadLine();

                int id = Students.Count + 1;

                Students.Add(new Student { Id = id, Name = name, Email = email });
                Console.WriteLine("Студент успешно добавлен\n");
            }

            public void CreateTeacher()
            {
                Console.Write("Введите имя преподавателя: ");
                string name = Console.ReadLine();

                Console.Write("Введите кафедру: ");
                string department = Console.ReadLine();

                int id = Teachers.Count + 1;

                Teachers.Add(new Teacher { Id = id, Name = name, Department = department });
                Console.WriteLine("Преподаватель учпешно добавлен\n");
            }

            public void CreateCourse()
            {
                Console.Write("Введите код курса (например CS101): ");
                string code = Console.ReadLine();

                Console.Write("Введите название курса: ");
                string title = Console.ReadLine();

                Courses.Add(new Course { Code = code, Title = title });
                Console.WriteLine("Курс создан\n");
            }
