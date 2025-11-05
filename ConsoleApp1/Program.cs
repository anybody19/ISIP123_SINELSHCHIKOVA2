using System;
using System.Collections.Generic;
using System.Linq;

namespace UniversitySystem
{
    // Класс Студент
    class Student
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Email { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }

    // Класс Преподавател
    class Teacher
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Department { get; set; }

        public List<Course> Courses { get; set; } = new List<Course>();
    }

    // Класс Курс
    class Course
    {
        public string Code { get; set; }
        public string Title { get; set; }
        public Teacher Teacher { get; set; }

        public List<Student> Students { get; set; } = new List<Student>();

