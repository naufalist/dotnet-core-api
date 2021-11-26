using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
  public class StudentDbContext : DbContext
  {
    public StudentDbContext(DbContextOptions<StudentDbContext> options) : base(options)
    {

    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      //modelBuilder.Entity<Student>()
      //    .HasData(
      //        new Student()
      //        {
      //          Id = 1,
      //          FirstName = "Zaky",
      //          LastName = "Ramadhan",
      //          IPK = 3.50M
      //        },
      //        new Student()
      //        {
      //          Id = 2,
      //          FirstName = "Devina",
      //          LastName = "Ramadhani",
      //          IPK = 4.00M
      //        },
      //        new Student()
      //        {
      //          Id = 3,
      //          FirstName = "Putri",
      //          LastName = "Larasati",
      //          IPK = 3.35M
      //        }
      //    );

      modelBuilder.UseSerialColumns(); // important for postgres!!
      modelBuilder.Entity<Student>(e => e.Property(o => o.IPK).HasColumnType("decimal(4,2)").HasConversion<decimal>());
    }

    public DbSet<Student> Student { get; set; }
  }
}
