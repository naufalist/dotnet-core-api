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
    public class SupervisorsController : Controller
    {
        private SupervisorService _supervisorService;

        public SupervisorsController(SupervisorService supervisorService)
        {
            _supervisorService = supervisorService;
        }

        [HttpGet]
        public IActionResult GetSupervisors()
        {
            return Ok(_supervisorService.GetSupervisors());
        }

        [HttpGet("with-delete")]
        public IActionResult GetSupervisorsWithDelete()
        {
            return Ok(_supervisorService.GetSupervisors(true));
        }

        [HttpGet("{supervisorId}")]
        public IActionResult GetSupervisor([FromRoute] int supervisorId)
        {
            var supervisor = _supervisorService.GetSupervisor(supervisorId);
            if (supervisor != null)
            {
                SupervisorOutput supervisorOutput= new()
                {
                    Name = supervisor.Name
                };
                return Ok(supervisorOutput);
            }

            return NotFound($"Supervisor with Id: {supervisorId} was not found.");
        }

        [HttpPost]
        public IActionResult AddSupervisor([FromBody] SupervisorOutput supervisorOutput)
        {
            //_projectService.AddProject(project);
            //return Ok();

            Supervisor supervisor = new()
            {
                Name = supervisorOutput.Name,
            };
            _supervisorService.AddSupervisor(supervisor);
            return Created(
                HttpContext.Request.Scheme + "://" +
                HttpContext.Request.Host + HttpContext.Request.Path + "/" +
                supervisor.Id, supervisor
            );
        }

        [HttpPatch("{supervisorId}")]
        public IActionResult EditSupervisor([FromRoute] int supervisorId, [FromBody] SupervisorOutput supervisor)
        {
            var existingSupervisor = _supervisorService.CheckSupervisorIfExists(supervisorId);
            if (existingSupervisor != false)
            {
                _supervisorService.EditSupervisor(supervisorId, supervisor);
                return GetSupervisor(supervisorId);
            }

            return NotFound($"Supervisor with Id: {supervisorId} was not found.");
        }

        [HttpDelete("{supervisorId}")]
        public IActionResult DeleteSupervisor([FromRoute] int supervisorId)
        {
            var supervisor = _supervisorService.GetSupervisor(supervisorId);
            if (supervisor != null)
            {
                bool supervisorDeleted =_supervisorService.DeleteSupervisor(supervisor);
                if (supervisorDeleted)
                {
                    return Ok($"Supervisor with Id: {supervisorId} deleted successfully.");
                } else
                {
                    return Ok($"Supervisor cannot be deleted. Probably it has relationship.");
                }
            }

            return NotFound($"Supervisor with Id: {supervisorId} was not found.");
        }

        [HttpDelete("delete-supervisor-by-id/{id}")]
        public IActionResult DeleteSupervisorById(int id)
        {
            _supervisorService.DeleteSupervisorById(id);
            return Ok();
        }
    }
}
