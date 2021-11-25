using StudentRestAPI.Models;
using StudentRestAPI.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.StudentData
{
    public class SqlStudentData : IStudentData
    {
        private readonly StudentDbContext _studentDbContext;
        private readonly IRedisCache _redisCache;

        public SqlStudentData(StudentDbContext studentDbContext, IRedisCache redisCache)
        {
            _studentDbContext = studentDbContext;
            _redisCache = redisCache;
        }
        public List<Student> GetStudents()
        {
            return _studentDbContext.Student.ToList();
            //throw new NotImplementedException();
        }

        public async Task<Student> GetStudent(int studentId, bool useRedis = true)
        {
            Student student = null;

            if (useRedis)
            {
                student = await _redisCache.Read<Student>($"student.id-{studentId}");
                if (student != null && student.Id > 0)
                {
                    return student;
                }
            }

            student = _studentDbContext.Student.Find(studentId);
            if (student != null && student.Id > 0)
            {
                _redisCache.Create($"student.id-{studentId}", student, new TimeSpan(0, 5, 0));
            }

            return student;

            //return _studentDbContext.Student.Find(studentId);
            //return _studentDbContext.Student.Where(student => student.Id.Equals(studentId)).FirstOrDefault();
        }

        public Student AddStudent(Student student)
        {
            _studentDbContext.Add(student);
            _studentDbContext.SaveChanges();
            return student;
        }

        public async Task<Student> EditStudent(int studentId, Student student)
        {
            var existingStudent = await GetStudent(studentId);
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.IPK = student.IPK;
            _studentDbContext.Student.Update(existingStudent);
            _studentDbContext.SaveChanges();
            _redisCache.Update($"student.id-{studentId}", existingStudent, new TimeSpan(0, 5, 0));
            return existingStudent;
        }

        public void DeleteStudent(Student student)
        {
            _studentDbContext.Student.Remove(student);
            _studentDbContext.SaveChanges();
            _redisCache.Delete($"student.id-{student.Id}");
        }

        public bool CheckStudentIfExists(int studentId)
        {
            return _studentDbContext.Student.Any(student => student.Id == studentId);
        }
    }
}
