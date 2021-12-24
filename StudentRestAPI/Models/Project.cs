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

        public List<Student> Students { get; set; }

        public List<StudentProject> Student_Projects { get; set; }
    }
}
