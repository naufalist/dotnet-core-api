using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class StudentInput
    {
        [MaxLength(30, ErrorMessage = "First name can only be filled with 30 characters.")]
        public string FirstName { get; set; }

        [MaxLength(30, ErrorMessage = "Last name can only be filled with 30 characters.")]
        public string LastName { get; set; }

        public decimal IPK { get; set; }

        // Id of Supervisor
        public int? SupervisorId { get; set; }

        // List<int> of Project Id(s)
        public List<int?> ProjectIds { get; set; }
    }

    public class StudentProjectIdsInput
    {
        public List<int> ProjectIds { get; set; }
    }
}
