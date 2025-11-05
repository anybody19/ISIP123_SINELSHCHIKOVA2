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

        / Менеджер для управления системой
    class UniversityManager
        {
            public List<Student> Students = new List<Student>();
            public List<Teacher> Teachers = new List<Teacher>();
            public List<Course> Courses = new List<Course>();

