using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRestAPI.Dtos;
using StudentRestAPI.Redis;
using StudentRestAPI.Services;
using StudentRestAPI.Models;

namespace StudentRestAPI.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly StudentService _studentService;
        private readonly SupervisorService _supervisorService;
        private readonly ProjectService _projectService;
        private readonly IRedisCache _redisCache;

        public StudentsController(StudentService studentService, SupervisorService supervisorService, ProjectService projectService, IRedisCache redisCache)
        {
            _studentService = studentService;
            _supervisorService = supervisorService;
            _projectService = projectService;
            _redisCache = redisCache;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetStudents()
        {
            return Ok(_studentService.GetStudents());
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            var student = await _studentService.GetStudent(studentId);
            if (student != null)
            {
                StudentOutput studentOutput = new()
                {
                    Id = student.Id,
                    FirstName = student.FirstName,
                    LastName = student.LastName,
                    IPK = student.IPK,
                    Supervisor = _supervisorService.GetNameById(Convert.ToInt32(student.SupervisorId)),
                    Projects = student.Projects.Select(project => project.Title).ToList()
                };
                return Ok(studentOutput);
            }
            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult AddStudent(StudentInput student)
        {
            // Initialize 3 values of Student (FirstName, LastName, IPK)
            Student _student = new()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                IPK = student.IPK
            };

            // (Optional) Add Supervisor to Student
            if (student.SupervisorId != null && student.SupervisorId > 0)
            {
                bool isExists = _supervisorService.CheckSupervisorIfExists(Convert.ToInt32(student.SupervisorId));
                if (isExists)
                {
                    _student.SupervisorId = student.SupervisorId;
                } else
                { 
                    return NotFound($"Supervisor with Id: {student.SupervisorId} was not found.");
                }
            }

            // (Optional) Add Project(s) to Student
            if (student.ProjectIds != null && student.ProjectIds.Count > 0)
            {
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

                    //var project = _dbContext.Project.Where(p => p.Id == projectId).FirstOrDefault();
                    bool isExists = _projectService.CheckProjectIfExists(Convert.ToInt32(projectId));
                    if (isExists)
                    {
                        Projects.Add(_projectService.GetProject(Convert.ToInt32(projectId)));
                    } else
                    {
                        return NotFound($"Project with Id: {Convert.ToInt32(projectId)} was not found.");
                    }
                }
                _student.Projects = Projects;
            }

            _studentService.AddStudent(_student);
            return Ok();
            //return Created(
            //    HttpContext.Request.Scheme + "://" +
            //    HttpContext.Request.Host + HttpContext.Request.Path + "/" +
            //    _student.Id, _student
            //);
        }

        [HttpPatch]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId, StudentInput student)
        {
            var existingStudent = _studentService.CheckStudentIfExists(studentId);
            if (existingStudent != false)
            {
                return Ok(await _studentService.EditStudent(studentId, student));
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpDelete]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _studentService.GetStudent(studentId);
            if (student != null)
            {
                _studentService.DeleteStudent(student);
                return Ok($"Student with Id: {studentId} deleted successfully.");
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpPost]
        [Route("api/[controller]/with-projects")]
        public IActionResult AddStudentWithProjects([FromBody] StudentWithProjectInput student)
        {
            _studentService.AddStudentWithProjects(student);
            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}/projects")]
        public IActionResult GetStudentWithProjects([FromRoute] int studentId)
        {
            var studentOutput = _studentService.GetStudentWithProjects(studentId);
            return Ok(studentOutput);
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}/supervisor")]
        public IActionResult GetStudentWithSupervisor([FromRoute] int studentId)
        {
            var studentOutput = _studentService.GetStudentWithSupervisor(studentId);
            return Ok(studentOutput);
        }

        [HttpPatch]
        [Route("api/[controller]/{studentId}/supervisor")]
        public async Task<IActionResult> AddSupervisorToStudent([FromRoute] int studentId, [FromBody] SupervisorInput supervisorInput)
        {
            bool existingSupervisor = _supervisorService.CheckSupervisorIfExists(supervisorInput.Id);
            if (existingSupervisor == false)
            {
                return NotFound($"Supervisor with Id: {supervisorInput.Id} was not found.");
            }

            bool existingStudent = _studentService.CheckStudentIfExists(studentId);
            if (existingStudent == false)
            {
                return NotFound($"Student with Id: {studentId} was not found.");
            }

            var supervisor = _supervisorService.GetSupervisor(supervisorInput.Id);
            return Ok(await _studentService.AddSupervisorToStudent(studentId, supervisor));
        }

        [HttpDelete]
        [Route("api/[controller]/{studentId}/supervisor")]
        public async Task<IActionResult> RemoveSupervisor(int studentId)
        {
            var student = await _studentService.GetStudent(studentId);
            if (student != null)
            {
                _studentService.RemoveSupervisor(student);
                return Ok($"Supervisor in Student {studentId} was removed successfully.");
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpDelete]
        [Route("api/[controller]/{studentId}/projects")]
        public async Task<IActionResult> RemoveProjects(int studentId, [FromBody] StudentProjectIdsInput studentProjectIdsInput)
        {
            var student = await _studentService.GetStudent(studentId, false);
            if (student != null)
            {
                foreach (var projectId in studentProjectIdsInput.ProjectIds)
                {
                    _studentService.RemoveProject(student, projectId);
                }
                return Ok($"Project was removed successfully.");
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }


    }
}
