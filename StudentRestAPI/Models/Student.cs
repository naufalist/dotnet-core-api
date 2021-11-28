using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class Student
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

        public int? SupervisorId { get; set; }
        public Supervisor Supervisor { get; set; }

        public List<Student_Project> Student_Projects { get; set; }
    }
}
