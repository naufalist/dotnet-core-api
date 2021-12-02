using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Services
{
    public class SupervisorService
    {
        private readonly AppDbContext _dbContext;

        public SupervisorService(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        public void AddSupervisor(SupervisorOutput supervisor)
        {
            var _supervisor = new Supervisor()
            {
                Name = supervisor.Name
            };
            _dbContext.Supervisor.Add(_supervisor);
            _dbContext.SaveChanges();
        }

        public List<Supervisor> GetSupervisors()
        {
            return _dbContext.Supervisor.ToList();
        }

        public Supervisor GetSupervisor(int supervisorId)
        {
            var supervisor = _dbContext.Supervisor.Find(supervisorId);
            if (supervisor != null && supervisor.Id > 0)
            {
                //ProjectOutput projectOutput = new ProjectOutput()
                //{
                //    Title = project.Title
                //};
                return supervisor;
            }
            return null;
        }

        public void AddSupervisor(Supervisor supervisor)
        {
            //var _project = new Project()
            //{
            //    Title = project.Title
            //};
            _dbContext.Supervisor.Add(supervisor);
            _dbContext.SaveChanges();
        }

        public Supervisor EditSupervisor(int supervisorId, SupervisorOutput supervisor)
        {
            var existingProject = _dbContext.Supervisor.Find(supervisorId);
            existingProject.Name = supervisor.Name;
            _dbContext.Supervisor.Update(existingProject);
            _dbContext.SaveChanges();
            return GetSupervisor(supervisorId);
        }

        public void DeleteSupervisor(Supervisor supervisor)
        {
            _dbContext.Supervisor.Remove(supervisor);
            _dbContext.SaveChanges();
        }

        public void DeleteSupervisorById(int id)
        {
            var _supervisor = _dbContext.Supervisor.FirstOrDefault(n => n.Id == id);
            if (_supervisor != null)
            {
                _dbContext.Supervisor.Remove(_supervisor);
                _dbContext.SaveChanges();
            }
        }

        public bool CheckSupervisorIfExists(int supervisorId)
        {
            return _dbContext.Supervisor.Any(supervisor => supervisor.Id == supervisorId);
        }
    }
}
