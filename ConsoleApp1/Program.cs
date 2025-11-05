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
                Console.WriteLine($"Студент {student.Name} успешно записан на курс {Title}.");
            }
            else
            {
                Console.WriteLine("Этот студент уже записан на данный курс.");
            }
        }
