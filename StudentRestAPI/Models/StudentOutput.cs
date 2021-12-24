using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class StudentOutput
    {
        [Key]
        public int Id { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage ="First name can only be filled with 30 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Last name can only be filled with 30 characters.")]
        public string LastName { get; set; }

        [Required]
        public decimal IPK { get; set; }

        // Name of Supervisor
        public string Supervisor { get; set; }

        // List<string> of Project Name(s)
        public List<string> Projects { get; set; }

        //public int? SupervisorId { get; set; }
        //public List<int> ProjectIds { get; set; }
    }

    public class StudentWithProjectInput
    {
        [Required]
        [MaxLength(30, ErrorMessage = "First name can only be filled with 30 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Last name can only be filled with 30 characters.")]
        public string LastName { get; set; }

        [Required]
        public decimal IPK { get; set; }

        public List<int> ProjectIds { get; set; }
    }


    public class StudentWithProjectsOutput
    {
        [Required]
        [MaxLength(30, ErrorMessage = "First name can only be filled with 30 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Last name can only be filled with 30 characters.")]
        public string LastName { get; set; }

        [Required]
        public decimal IPK { get; set; }

        //public string SupervisorName { get; set; }
        public List<string> ProjectNames { get; set; }
    }

    public class StudentWithSupervisorOutput
    {
        [Required]
        [MaxLength(30, ErrorMessage = "First name can only be filled with 30 characters.")]
        public string FirstName { get; set; }

        [Required]
        [MaxLength(30, ErrorMessage = "Last name can only be filled with 30 characters.")]
        public string LastName { get; set; }

        [Required]
        public decimal IPK { get; set; }

        public string SupervisorName { get; set; }
    }
}
