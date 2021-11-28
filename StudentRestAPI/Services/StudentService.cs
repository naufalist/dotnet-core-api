using StudentRestAPI.Models;
using StudentRestAPI.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Services
{
    public class StudentService
    {
        private readonly AppDbContext _dbContext;
        private readonly IRedisCache _redisCache;

        public StudentService(AppDbContext AppDbContext, IRedisCache redisCache)
        {
            _dbContext = AppDbContext;
            _redisCache = redisCache;
        }

        public List<Student> GetStudents()
        {
            return _dbContext.Student.ToList();
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

            student = _dbContext.Student.Find(studentId);
            if (student != null && student.Id > 0)
            {
                _redisCache.Create($"student.id-{studentId}", student, new TimeSpan(0, 5, 0));
            }

            return student;

            //return _dbContext.Student.Find(studentId);
            //return _dbContext.Student.Where(student => student.Id.Equals(studentId)).FirstOrDefault();
        }

        public Student AddStudent(Student student)
        {
            _dbContext.Add(student);
            _dbContext.SaveChanges();
            return student;
        }

        public async Task<Student> EditStudent(int studentId, Student student)
        {
            var existingStudent = await GetStudent(studentId);
            existingStudent.FirstName = student.FirstName;
            existingStudent.LastName = student.LastName;
            existingStudent.IPK = student.IPK;
            existingStudent.SupervisorId = student.SupervisorId;
            _dbContext.Student.Update(existingStudent);
            _dbContext.SaveChanges();
            _redisCache.Update($"student.id-{studentId}", existingStudent, new TimeSpan(0, 5, 0));
            return existingStudent;
        }

        public void DeleteStudent(Student student)
        {
            _dbContext.Student.Remove(student);
            _dbContext.SaveChanges();
            _redisCache.Delete($"student.id-{student.Id}");
        }

        public bool CheckStudentIfExists(int studentId)
        {
            return _dbContext.Student.Any(student => student.Id == studentId);
        }

        public void AddStudentWithProjects(StudentOutput student)
        {
            var _student = new Student()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                IPK = student.IPK,
                SupervisorId = student.SupervisorId
            };

            _dbContext.Student.Add(_student);
            _dbContext.SaveChanges();

            foreach (var projectId in student.ProjectIds)
            {
                var _student_project = new Student_Project()
                {
                    StudentId = _student.Id,
                    ProjectId = projectId
                };
                _dbContext.Student_Project.Add(_student_project);
                _dbContext.SaveChanges();
            }
        }

        public StudentWithProjectsOutput GetStudentWithProjects(int studentId)
        {
            var _studentWithProjects = _dbContext.Student
                .Where(s => s.Id == studentId)
                .Select(student => new StudentWithProjectsOutput()
                    {
                        FirstName = student.FirstName,
                        LastName = student.LastName,
                        IPK = student.IPK,
                        //SupervisorName = student.Supervisor.Name,
                        ProjectNames = student.Student_Projects.Select(sp => sp.Project.Title).ToList()
                    }
                ).FirstOrDefault();

            return _studentWithProjects;
        }
    }
}
