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
    public class ProjectsController : Controller
    {
        private ProjectService _projectService;

        public ProjectsController(ProjectService projectService)
        {
            _projectService = projectService;
        }

        [HttpGet]
        public IActionResult GetProjects()
        {
            return Ok(_projectService.GetProjects());
        }

        [HttpGet("{projectId}")]
        public IActionResult GetProject([FromRoute] int projectId)
        {
            var project = _projectService.GetProject(projectId);
            if (project != null)
            {
                ProjectOutput projectOutput = new()
                {
                    Title = project.Title
                };
                return Ok(projectOutput);
            }

            return NotFound($"Project with Id: {projectId} was not found.");
        }

        [HttpPost]
        public IActionResult AddProject([FromBody] ProjectOutput projectOutput)
        {
            //_projectService.AddProject(project);
            //return Ok();

            Project project = new()
            {
                Title = projectOutput.Title,
            };
            _projectService.AddProject(project);
            return Created(
                HttpContext.Request.Scheme + "://" +
                HttpContext.Request.Host + HttpContext.Request.Path + "/" +
                project.Id, project
            );
        }

        [HttpPatch("{projectId}")]
        public IActionResult EditStudent(int projectId, ProjectOutput project)
        {
            var existingProject = _projectService.CheckProjectIfExists(projectId);
            if (existingProject != false)
            {
                _projectService.EditProject(projectId, project);
                return GetProject(projectId);
            }

            return NotFound($"Project with Id: {projectId} was not found.");
        }

        [HttpDelete("{projectId}")]
        public IActionResult DeleteProject([FromRoute] int projectId)
        {
            var project = _projectService.GetProject(projectId);
            if (project != null)
            {
                _projectService.DeleteProject(project);
                return Ok($"Project with Id: {projectId} deleted successfully.");
            }

            return NotFound($"Project with Id: {projectId} was not found.");
        }

    }
}
