using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Services
{
    public class ProjectService
    {
        private readonly AppDbContext _dbContext;

        public ProjectService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public List<Project> GetProjects()
        {
            return _dbContext.Project.ToList();
        }

        public Project GetProject(int projectId)
        {
            var project = _dbContext.Project.Find(projectId);
            if (project != null && project.Id > 0)
            {
                //ProjectOutput projectOutput = new ProjectOutput()
                //{
                //    Title = project.Title
                //};
                return project;
            }
            return null;
        }

        public void AddProject(Project project)
        {
            //var _project = new Project()
            //{
            //    Title = project.Title
            //};
            _dbContext.Project.Add(project);
            _dbContext.SaveChanges();
        }

        public Project EditProject(int projectId, ProjectOutput project)
        {
            var existingProject = _dbContext.Project.Find(projectId);
            existingProject.Title = project.Title;
            _dbContext.Project.Update(existingProject);
            _dbContext.SaveChanges();
            return GetProject(projectId);
        }

        public void DeleteProject(Project project)
        {
            _dbContext.Project.Remove(project);
            _dbContext.SaveChanges();
        }

        public bool CheckProjectIfExists(int projectId)
        {
            return _dbContext.Project.Any(project => project.Id == projectId);
        }
    }
}
