using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.DependencyInjection;
using StudentRestAPI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace StudentRestAPI.Seeders
{
    public class StudentSeeder
    {
        public static void Seed(IApplicationBuilder applicationBuilder)
        {
            using (var serviceScope = applicationBuilder.ApplicationServices.CreateScope())
            {
                var context = serviceScope.ServiceProvider.GetService<StudentDbContext>();

                if (context.Student.Any())
                {
                    context.Student.AddRange(
                        new Student()
                        {
                            Id = 1,
                            FirstName = "Zaky",
                            LastName = "Ramadhan",
                            IPK = 3.50M
                        },
                        new Student()
                        {
                            Id = 2,
                            FirstName = "Devina",
                            LastName = "Ramadhani",
                            IPK = 4.00M
                        },
                        new Student()
                        {
                            Id = 3,
                            FirstName = "Putri",
                            LastName = "Larasati",
                            IPK = 3.35M
                        }
                    );

                    context.SaveChanges();
                }
            }
        }
    }
}
