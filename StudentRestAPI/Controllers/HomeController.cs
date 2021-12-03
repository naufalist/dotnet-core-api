using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace StudentRestAPI.Controllers
{
    [Route("/")]
    [ApiController]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        public IActionResult Root()
        {
            object message = new
            {
                message = "Welcome to the jungle :)"
            };
            return Ok(JsonSerializer.Serialize(message));
        }
    }
}
