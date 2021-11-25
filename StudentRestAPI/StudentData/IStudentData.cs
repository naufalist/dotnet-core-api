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

        Task<Student> GetStudent(int studentId, bool useRedis = true);

        Student AddStudent(Student student);

        Task<Student> EditStudent(int studentId, Student student);

        void DeleteStudent(Student student);

        bool CheckStudentIfExists(int studentId);

    }
}
