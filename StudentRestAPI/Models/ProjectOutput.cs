using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class ProjectOutput
    {
        public string Title { get; set; }
    }

    public class ProjectWithStudentsOutput
    {
        public string Title { get; set; }
        public List<string> StudentNames { get; set; }
    }
}
