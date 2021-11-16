using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.StudentData
{
    public class SqlStudentData : IStudentData
    {
        private readonly StudentDbContext _studentDbContext;
        public SqlStudentData(StudentDbContext studentDbContext)
        {
            _studentDbContext = studentDbContext;
        }
        public List<Student> GetStudents()
        {
            return _studentDbContext.Student.ToList();
            //throw new NotImplementedException();
        }

        public Student GetStudent(int studentId)
        {
            return _studentDbContext.Student.Find(studentId);
            //return _studentDbContext.Student.Where(student => student.Id.Equals(studentId)).FirstOrDefault();
        }

        public Student AddStudent(Student student)
        {
            _studentDbContext.Add(student);
            _studentDbContext.SaveChanges();
            return student;
        }

        public Student EditStudent(int studentId, Student student)
        {
            var existingStudent = GetStudent(studentId);
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.IPK = student.IPK;
            _studentDbContext.Student.Update(existingStudent);
            _studentDbContext.SaveChanges();
            return existingStudent;
        }

        public void DeleteStudent(Student student)
        {
            _studentDbContext.Student.Remove(student);
            _studentDbContext.SaveChanges();
        }


    }
}
