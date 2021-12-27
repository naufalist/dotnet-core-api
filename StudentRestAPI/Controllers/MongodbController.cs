using Microsoft.AspNetCore.Mvc;
using StudentRestAPI.Models;
using StudentRestAPI.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class MongodbController : Controller
    {
        private readonly StudentService _studentService;
        private readonly SupervisorService _supervisorService;

        public MongodbController(StudentService studentService, SupervisorService supervisorService)
        {
            _studentService = studentService;
            _supervisorService = supervisorService;
        }

        [HttpGet]
        public IActionResult Get()
        {
            List<StudentOutput> students = _studentService.MongoDbGetAll();
            return Ok(students);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            StudentOutput student = await _studentService.MongoDbGetById(Convert.ToInt32(id));
            return Ok(student);
        }

        [HttpPost]
        public IActionResult Create()
        {
            List<StudentOutput> students = _studentService.GetStudents();

            foreach (StudentOutput student in students)
            {
                bool created = _studentService.MongoDbCreate(student);
                if (!created)
                {
                    return Ok("failed");
                }
            };

            return Ok("finish");
        }

        [HttpPost("{studentId}")]
        public async Task<IActionResult> Create([FromRoute] int studentId)
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

                bool created = _studentService.MongoDbCreate(studentOutput);
                if (!created)
                {
                    return Ok(new { status = false });
                }

                return Ok(new { status = true, data = studentOutput });

            }
            return NotFound($"Student with Id: {studentId} was not found.");
        }

        [HttpPatch("{id}")]
        public IActionResult Update([FromRoute] string id)
        {
            /*
             * Partial Update
             * params: [FromRoute] string id
             */
            var update = _studentService.MongoDbUpdate(Convert.ToInt32(id));

            /*
             * Full Update
             * params: [FromRoute] string id, [FromBody] StudentInput studentInput
             */
            //StudentOutput studentOutput = new StudentOutput()
            //{
            //    Id = Convert.ToInt32(id),
            //    FirstName = studentInput.FirstName,
            //    LastName = studentInput.LastName,
            //    IPK = studentInput.IPK
            //    // supervisor Id and project Id must be implement here!
            //    // because of the DBs not created yet, i couldn't implement.
            //    // for now, those values will be updated with NULL
            //};
            //var update = _studentService.MongoDbUpdate(Convert.ToInt32(id), studentOutput);

            //{
            //    "isAcknowledged": true,
            //    "isModifiedCountAvailable": true,
            //    "matchedCount": 1,
            //    "modifiedCount": 1,
            //    "upsertedId": null
            //}
            return Ok(update);
        }

        [HttpDelete("{id}")]
        public IActionResult Delete([FromRoute] string id)
        {
            var delete = _studentService.MongoDbDelete(Convert.ToInt32(id));
            return Ok(delete);
            //{
            //    "deletedCount": 1, // 0 if not found or has been deleted before
            //    "isAcknowledged": true
            //}
        }
    }
}
