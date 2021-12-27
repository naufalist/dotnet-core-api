using Microsoft.EntityFrameworkCore;
using MongoDB.Bson;
using MongoDB.Driver;
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
        private readonly MongodbContext _mongodbContext;
        private readonly IRedisCache _redisCache;
        private readonly ProjectService _projectService;
        private readonly SupervisorService _supervisorService;

        public StudentService(AppDbContext AppDbContext, MongodbContext mongodbContext, ProjectService projectService, SupervisorService supervisorService, IRedisCache redisCache)
        {
            _dbContext = AppDbContext;
            _mongodbContext = mongodbContext;
            _projectService = projectService;
            _supervisorService = supervisorService;
            _redisCache = redisCache;
        }

        public List<StudentOutput> GetStudents()
        {
            var student = _dbContext.Student.Select(student => new StudentOutput()
            {
                Id = student.Id,
                FirstName = student.FirstName,
                LastName = student.LastName,
                IPK = student.IPK,
                Supervisor = _dbContext.Supervisor.Where(supervisor => supervisor.Id == student.SupervisorId).Select(supervisor => supervisor.Name).FirstOrDefault(),
                Projects = student.Projects.Select(project => project.Title).ToList()
            })
                .ToList();
            return student;
            //return _dbContext.Student.ToList();
            //return _dbContext.Student
            //    .Join(
            //        _dbContext.Supervisor,
            //        student => student.SupervisorId,
            //        supervisor => supervisor.Id,
            //        (student, supervisor) => new
            //        {
            //            StudentTest = student,
            //            SupervisorTest = supervisor
            //        })
            //    .ToList();
            //return _dbContext.Student
            //    .Include(s => s.Supervisor)
            //    .Include(s => s.Projects)
            //    .ToList();

                //.SelectMany(p => p.Projects.DefaultIfEmpty(), (s, p) => new
                //{
                //    Title = p.Title,
                //})
                //.ToList();
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

            //student = _dbContext.Student.Find(studentId);
            student = _dbContext.Student.Include(s => s.Projects).FirstOrDefault(s => s.Id == studentId);
            if (student != null && student.Id > 0)
            {
                StudentOutput studentOutput = _dbContext.Student.Select(student => new StudentOutput()
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    IPK = student.IPK,
                    Supervisor = _dbContext.Supervisor.Where(supervisor => supervisor.Id == student.SupervisorId).Select(supervisor => supervisor.Name).FirstOrDefault(),
                    //Supervisor = _supervisorService.GetNameById(Convert.ToInt32(student.SupervisorId)), // ga bisa
                    Projects = student.Projects.Select(project => project.Title).ToList()
                }).FirstOrDefault(s => s.Id == student.Id);
                _redisCache.Create($"student.id-{studentId}", studentOutput, new TimeSpan(0, 5, 0));
            }

            return student;
            //return student;

            //return _dbContext.Student.Find(studentId);
            //return _dbContext.Student.Where(student => student.Id.Equals(studentId)).FirstOrDefault();
        }

        public Student AddStudent(Student student)
        {
            _dbContext.Add(student);
            _dbContext.SaveChanges();
            return student;
        }

        public async Task<StudentOutput> EditStudent(int studentId, StudentInput student)
        {
            // get Student from SQL DB
            var existingStudent = await GetStudent(studentId, false);

            if (student.FirstName != null && student.FirstName is string)
            {
                existingStudent.FirstName = student.FirstName;
            }

            if (student.LastName != null && student.LastName is string)
            {
                existingStudent.LastName = student.LastName;
            }

            if (student.IPK > 0 && student.IPK <= 4.00m)
            {
                existingStudent.IPK = student.IPK;
            }

            if (student.SupervisorId != null && student.SupervisorId > 0)
            {
                bool isExists = _supervisorService.CheckSupervisorIfExists(Convert.ToInt32(student.SupervisorId));
                if (isExists)
                {
                    existingStudent.SupervisorId = student.SupervisorId;
                }
                else
                {
                    return null;
                }
            }

            if (student.ProjectIds != null && student.ProjectIds.Count > 0)
            {
                List<Project> Projects = new();
                foreach (var projectId in student.ProjectIds)
                {
                    bool isExists = _projectService.CheckProjectIfExists(Convert.ToInt32(projectId));
                    if (isExists)
                    {
                        Projects.Add(_projectService.GetProject(Convert.ToInt32(projectId)));
                    }
                    else
                    {
                        return null;
                    }
                }
                existingStudent.Projects = Projects;
            }

            _dbContext.Student.Update(existingStudent);
            _dbContext.SaveChanges();

            // Update to redis using StudentOutput
            StudentOutput studentOutput = new()
            {
                Id = existingStudent.Id,
                FirstName = existingStudent.FirstName,
                LastName = existingStudent.LastName,
                IPK = existingStudent.IPK,
                Supervisor = _supervisorService.GetNameById(Convert.ToInt32(existingStudent.SupervisorId)),
                Projects = existingStudent.Projects.Select(project => project.Title).ToList()
            };

            _redisCache.Update($"student.id-{studentId}", studentOutput, new TimeSpan(0, 5, 0));
            return studentOutput;
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
                FirstName = student.FirstName,
                LastName = student.LastName,
                IPK = student.IPK,
                //SupervisorId = student.SupervisorId
            };

            _dbContext.Student.Add(_student);
            _dbContext.SaveChanges();

            List<Project> Projects = new();
            foreach (var projectId in student.ProjectIds)
            {
                //var _student_project = new ProjectStudent()
                //{
                //    StudentId = _student.Id,
                //    ProjectId = projectId
                //};
                //_dbContext.Student_Project.Add(_student_project);
                //_dbContext.SaveChanges();
                var project = _dbContext.Project.Where(p => p.Id == projectId).FirstOrDefault();
                Projects.Add(project);
            }

            _student.Projects = Projects;
            _dbContext.Student.Update(_student);
            _dbContext.SaveChanges();
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
                        //ProjectNames = student.Student_Projects.Select(sp => sp.Project.Title).ToList()
                        ProjectNames = student.Projects.Select(p => p.Title).ToList()
                    }
                ).FirstOrDefault();

            return _studentWithProjects;
        }

        public StudentWithSupervisorOutput GetStudentWithSupervisor(int studentId)
        {
            var _studentWithProjects = _dbContext.Student
                .Where(s => s.Id == studentId)
                .Select(student => new StudentWithSupervisorOutput()
                {
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    IPK = student.IPK,
                    SupervisorName = student.Supervisor.Name
                }
                ).FirstOrDefault();

            return _studentWithProjects;
        }

        public async Task<Student> AddSupervisorToStudent(int studentId, Supervisor supervisor)
        {
            var existingStudent = await GetStudent(studentId, false);
            existingStudent.SupervisorId = supervisor.Id;
            _dbContext.Student.Update(existingStudent);
            _dbContext.SaveChanges();
            _redisCache.Update($"student.id-{studentId}", existingStudent, new TimeSpan(0, 5, 0));
            return existingStudent;
        }
        public void RemoveSupervisor(Student student)
        {
            student.SupervisorId = null;
            _dbContext.Student.Update(student);
            _dbContext.SaveChanges();
            _redisCache.Delete($"student.id-{student.Id}");
        }

        public void RemoveProject(Student student, int projectId)
        {
            Project project = _projectService.GetProject(projectId);
            student.Projects.Remove(project);
            _dbContext.Student.Update(student);
            _dbContext.SaveChanges();            
        }

        public List<StudentOutput> MongoDbGetAll()
        {
            return _mongodbContext.Students.Find(student => true).ToList();
        }

        public async Task<StudentOutput> MongoDbGetById(int id)
        {
            return await _mongodbContext.Students.Find(student => student.Id == id).SingleOrDefaultAsync();
        }

        public bool MongoDbCreate(StudentOutput studentOutput)
        {
            try
            {
                _mongodbContext.Students.InsertOne(studentOutput);
            }
            catch (Exception)
            {
                return false;
            }
            return true;
        }

        public UpdateResult MongoDbUpdate(int id)
        {
            /*
             * Partial update using "UpdateOne"
             * params: int id
             * return type: UpdateResult
             */
            var filter = Builders<StudentOutput>.Filter.Eq(s => s.Id, id);
            var update = Builders<StudentOutput>.Update.Set(s => s.LastName, "Elizabeth");
            //var options = new UpdateOptions { IsUpsert = true };
            //return _mongodbContext.Students.UpdateOne(filter, update, options);
            return _mongodbContext.Students.UpdateOne(filter, update);

            /*
             * Full Update
             * params: int id, StudentOutput studentOutput
             * return type: ReplaceOneResult
             */
            //return _mongodbContext.Students.ReplaceOne(student => student.Id == id, studentOutput);
        }

        public DeleteResult MongoDbDelete(int id)
        {
            return _mongodbContext.Students.DeleteOne(s => s.Id == id);
        }
    }
}
