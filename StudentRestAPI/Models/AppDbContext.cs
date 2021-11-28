using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Models
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
        {
        
        }
        
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            //modelBuilder.Entity<Student>()
            //    .HasData(
            //        new Student()
            //        {
            //            Id = 1,
            //            FirstName = "Zaky",
            //            LastName = "Ramadhan",
            //            IPK = 3.50M
            //        },
            //        new Student()
            //        {
            //            Id = 2,
            //            FirstName = "Devina",
            //            LastName = "Ramadhani",
            //            IPK = 4.00M
            //        },
            //        new Student()
            //        {
            //            Id = 3,
            //            FirstName = "Putri",
            //            LastName = "Larasati",
            //            IPK = 3.35M
            //        }
            //    );

            modelBuilder.Entity<Student>(e => e.Property(o => o.IPK).HasColumnType("decimal(4,2)").HasConversion<decimal>());
            
            modelBuilder.Entity<Student_Project>()
                .HasOne(s => s.Student)
                .WithMany(sp => sp.Student_Projects)
                .HasForeignKey(si => si.StudentId);
            
            modelBuilder.Entity<Student_Project>()
                .HasOne(s => s.Project)
                .WithMany(sp => sp.Student_Projects)
                .HasForeignKey(si => si.StudentId);

            modelBuilder.UseSerialColumns(); // important for postgres!!
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Project> Project { get; set; }
        public DbSet<Student_Project> Student_Project { get; set; }
        public DbSet<Supervisor> Supervisor { get; set; }

    }
}
