using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.StudentData
{
    public class MockStudentData : IStudentData
    {

        private readonly List<Student> students = new()
        {
            new Student()
            {
                Id = 1,
                FirstName = "Zaky",
                LastName = "Ramadhan",
                IPK = 3.50M
            },
            new Student()
            {
                Id = 2,
                FirstName = "Devina",
                LastName = "Ramadhani",
                IPK = 4.00M
            },
            new Student()
            {
                Id = 3,
                FirstName = "Putri",
                LastName = "Larasati",
                IPK = 3.35M
            },
        };
        public List<Student> GetStudents()
        {
            return students;
        }
        public Student GetStudent(int studentId)
        {
            return students.SingleOrDefault(student => student.Id == studentId);
        }

        public Student AddStudent(Student student)
        {
            student.Id = students.Last().Id + 1;
            students.Add(student);
            return student;
        }
        public Student EditStudent(int studentId, Student student)
        {
            var existingStudent = GetStudent(studentId);
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.IPK = student.IPK;
            return existingStudent;
        }

        public void DeleteStudent(Student student)
        {
            students.Remove(student);
        }

    }
}
