using StudentRestAPI.Models;
using StudentRestAPI.Redis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.StudentData
{
    public class SqlStudentData
    {
        private readonly AppDbContext _dbContext;
        private readonly IRedisCache _redisCache;

        public SqlStudentData(AppDbContext studentDbContext, IRedisCache redisCache)
        {
            _dbContext = studentDbContext;
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

        public void AddStudentWithProjects(StudentWithProjectInput student)
        {
            var _student = new Student()
            {
                FirstName = "Naufal",
                LastName = "Wafi",
                IPK = 3.80M,
                //SupervisorId = student.SupervisorId
            };

            _dbContext.Student.Add(_student);
            _dbContext.SaveChanges();

            foreach (var id in student.ProjectIds)
            {
                var _student_project = new StudentProject()
                {
                    StudentId = _student.Id,
                    ProjectId = id
                };
                //_dbContext.Student_Project.Add(_student_project);
                _dbContext.SaveChanges();
            }
        }

        public StudentWithProjectsOutput GetStudentById(int studentId)
        {
            var _studentWithProjects = _dbContext.Student
                .Where(n => n.Id == studentId)
                .Select(student => new StudentWithProjectsOutput()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    IPK = student.IPK,
                    //SupervisorName = student.Supervisor.Name,
                    //ProjectNames = student.Student_Projects.Select(n => n.Project.Title).ToList()
                }).FirstOrDefault();

            return _studentWithProjects;
        }
    }
}
