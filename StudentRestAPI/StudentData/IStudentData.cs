using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.StudentData
{
    public interface IStudentData
    {
        List<Student> GetStudents();

        Student GetStudent(int studentId);

        Student AddStudent(Student student);

        Student EditStudent(int studentId, Student student);

        void DeleteStudent(Student student);

    }
}
