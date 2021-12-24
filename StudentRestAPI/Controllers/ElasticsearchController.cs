using Microsoft.AspNetCore.Mvc;
using Nest;
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
    public class ElasticsearchController : Controller
    {
        private readonly IElasticClient _elasticClient;
        private readonly StudentService _studentService;

        public ElasticsearchController(IElasticClient elasticClient, StudentService studentService)
        {
            _elasticClient = elasticClient;
            _studentService = studentService;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(string id)
        {
            var response = await _elasticClient.GetAsync<StudentOutput>(id, s => s.Index("students"));
            if (response.IsValid)
            {
                return Ok(response.Source);
                //return Ok(response.Found); // true
            }
            //return Ok(response.Found); // false
            return NoContent();
        }

        [HttpPost]
        public async Task<IActionResult> Create()
        {
            List<StudentOutput> students = _studentService.GetStudents();

            /*
             * Insert Many
             */
            var asyncBulkIndexResponse = await _elasticClient.BulkAsync(b => b
                .Index("students")
                .IndexMany(students)
            );
            return Ok(asyncBulkIndexResponse);

            /*
             * Insert One
             */
            //Student student = new()
            //{
            //    FirstName = "Anggita",
            //    LastName = "Laksani",
            //    IPK = 3.95M,
            //};
            //var indexResponse = await _elasticClient.IndexAsync(student, b => b.Index("students").Id(2));
            //if (!indexResponse.IsValid)
            //{
            //    return Ok("Response invalid");
            //}
            //return Ok(indexResponse);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> Update([FromRoute] string id, [FromBody] StudentInput studentInput)
        {
            // must full payload!
            var response = await _elasticClient.UpdateAsync<StudentOutput, object>(id, d => d.Index("students").Doc(studentInput));

            return Ok(response);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(string id)
        {
            var response = await _elasticClient.DeleteAsync<StudentOutput>(id, b => b.Index("students"));

            return Ok(response);
        }
    }
}
