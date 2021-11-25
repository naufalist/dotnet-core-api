using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StudentRestAPI.StudentData;
using StudentRestAPI.Models;
using StudentRestAPI.Dtos;
using StudentRestAPI.Redis;

namespace StudentRestAPI.Controllers
{
    [ApiController]
    public class StudentsController : ControllerBase
    {
        private readonly IStudentData _studentData;
        private readonly IRedisCache _redisCache;

        public StudentsController(IStudentData studentData, IRedisCache redisCache)
        {
            _studentData = studentData;
            _redisCache = redisCache;
        }

        [HttpGet]
        [Route("api/[controller]")]
        public IActionResult GetStudents()
        {
            return Ok(_studentData.GetStudents());
        }

        [HttpGet]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> GetStudent(int studentId)
        {
            var student = await _studentData.GetStudent(studentId);
            if (student != null)
            {
                return Ok(student);
            }
            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpPost]
        [Route("api/[controller]")]
        public IActionResult AddStudent(AddStudentDto addStudentDto)
        {
            Student student = new()
            {
                FirstName = addStudentDto.FirstName,
                LastName = addStudentDto.LastName,
                IPK = addStudentDto.IPK
            };
            _studentData.AddStudent(student);
            return Created(
                HttpContext.Request.Scheme + "://" +
                HttpContext.Request.Host + HttpContext.Request.Path + "/" +
                student.Id, student
            );
        }

        [HttpPatch]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> EditStudent(int studentId, Student student)
        {
            var existingStudent = _studentData.CheckStudentIfExists(studentId);
            if (existingStudent != false)
            {
                return Ok(await _studentData.EditStudent(studentId, student));
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpDelete]
        [Route("api/[controller]/{studentId}")]
        public async Task<IActionResult> DeleteStudent(int studentId)
        {
            var student = await _studentData.GetStudent(studentId);
            if (student != null)
            {
                _studentData.DeleteStudent(student);
                return Ok($"Student with Id: {studentId} deleted successfully.");
            }

            return NotFound($"Student with Id: {studentId} was not found.");
        }
    }
}
