using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class Project
    {
        public int Id { get; set; }
        public string Title { get; set; }

        public List<Student_Project> Student_Projects { get; set; }
    }
}
