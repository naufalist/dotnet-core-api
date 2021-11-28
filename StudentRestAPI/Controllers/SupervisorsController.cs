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

        [HttpPost("add-supervisor")]
        public IActionResult AddSupervisor([FromBody] SupervisorOutput supervisor)
        {
            _supervisorService.AddSupervisor(supervisor);
            return Ok();
        }
    }
}
