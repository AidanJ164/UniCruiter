using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Linq;
using Team5.Data;
using Team5.Models;

namespace MvcStudent.Models
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new Team5Context(
                serviceProvider.GetRequiredService<
                    DbContextOptions<Team5Context>>()))
            {
                // Look for any Students.
                if (context.Student.Any())
                {
                    return;   // DB has been seeded
                }

                context.Student.AddRange(
                    new Student
                    {
                        Id = 1,
                        FirstName = "Joshua",
                        LastName = "Crawford",
                        Major = "Computer Engineering",
                        GradDate = DateTime.Today
                    }
                );
                context.SaveChanges();
            }
        }
    }
}
