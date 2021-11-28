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
        private readonly IRedisCache _redisCache;

        public StudentsController(StudentService studentService, IRedisCache redisCache)
        {
            _studentService = studentService;
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
                return Ok(student);
            }
            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult AddStudent(StudentOutput student)
        {
            Student _student = new()
            {
                FirstName = student.FirstName,
                LastName = student.LastName,
                IPK = student.IPK
            };
            _studentService.AddStudent(_student);
            return Created(
                HttpContext.Request.Scheme + "://" +
                HttpContext.Request.Host + HttpContext.Request.Path + "/" +
                _student.Id, _student
            );
        }

        [HttpPatch]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId, Student student)
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
        public IActionResult AddStudentWithProjects([FromBody] StudentOutput student)
        {
            _studentService.AddStudentWithProjects(student);
            return Ok();
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}/with-projects")]
        public IActionResult GetStudentWithProjects([FromRoute] int studentId)
        {
            var studentOutput = _studentService.GetStudentWithProjects(studentId);
            return Ok(studentOutput);
        }
    }
}
