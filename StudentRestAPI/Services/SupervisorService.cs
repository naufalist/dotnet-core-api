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
    }
}
