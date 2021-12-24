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

            modelBuilder.Entity<Student>().HasOne(s => s.Supervisor).WithMany(sp => sp.Students);

            //modelBuilder.Entity<Student_Project>()
            //    .HasOne(s => s.Student)
            //    .WithMany(sp => sp.Student_Projects)
            //    .HasForeignKey(si => si.StudentId);
            
            //modelBuilder.Entity<Student_Project>()
            //    .HasOne(s => s.Project)
            //    .WithMany(sp => sp.Student_Projects)
            //    .HasForeignKey(si => si.ProjectId);

            modelBuilder.Entity<Supervisor>().HasQueryFilter(e => e.DeletedAt == null);

            modelBuilder.UseSerialColumns(); // important for postgres!!
        }

        public int SaveChanges1()
        {
            foreach (var entry in ChangeTracker.Entries())
            {
                var entity = entry.Entity;
                if (entry.State == EntityState.Deleted && entity is ISoftDelete)
                {
                    entry.State = EntityState.Modified;
                    entity.GetType().GetProperty("DeletedAt").SetValue(entity, DateTime.Now);
                }
            }

            return base.SaveChanges();
        }

        /// <summary>
        /// Marks any "Removed" Entities as "Unchanged" and then sets the Db [DeletedAt] with DateTime.Now
        /// </summary>
        public override int SaveChanges()
        {
            ChangeTracker.DetectChanges();

            var markedAsDeleted = ChangeTracker.Entries().Where(x => x.State == EntityState.Deleted);

            foreach (var item in markedAsDeleted)
            {
                if (item.Entity is ISoftDelete entity)
                {
                    item.State = EntityState.Unchanged;
                    entity.DeletedAt = DateTime.Now;
                }
            }

            return base.SaveChanges();
        }

        public DbSet<Student> Student { get; set; }
        public DbSet<Project> Project { get; set; }
        //public DbSet<ProjectStudent> Student_Project { get; set; }
        public DbSet<Supervisor> Supervisor { get; set; }

    }
}
