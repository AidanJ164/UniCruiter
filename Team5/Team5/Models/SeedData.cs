using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
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

                SeedStudents(context);
            }
        }

        private static void SeedStudents(Team5Context context)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // NOTE:
            // Use the following to get the exact resource name
            // to be assigned to the resourceName variable below.
            string[] resourceNames = assembly.GetManifestResourceNames();

            string resourceName = "Team5.Data.SeedData.Students.csv";
            string line;

            using (Stream stream = assembly.GetManifestResourceStream(resourceName))
            using (StreamReader reader = new StreamReader(stream))
            {
                string header = reader.ReadLine();
                while ((line = reader.ReadLine()) != null)
                {
                    // Writes to the Output Window.
                    Debug.WriteLine(line);

                    // Logic to parse the line, separate by comma(s), and assign fields
                    // to the Student model.

                    string[] values = line.Split(",");

                    context.Student.Add(
                        new Student
                        {
                            Id = int.Parse(values[0]),
                            FirstName = values[1],
                            LastName = values[2],
                            Major = values[3],
                            GradDate = DateTime.Parse(values[4])
                        }
                    );
                }
                context.SaveChanges();
            }
        }
    }
}
