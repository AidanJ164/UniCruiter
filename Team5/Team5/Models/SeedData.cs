using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using Team5.Data;
using Team5.Models;
using Microsoft.AspNetCore.Identity;

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
                // Look for any Users.
                if (!context.Users.Any())
                {
                    SeedUsers(context);
                    context.SaveChanges();
                }


                // Look for any Students.
                if (context.Student.Any())
                {
                    return;   // DB has been seeded
                }

                SeedStudents(context);
            }
        }

        private static void SeedUsers(Team5Context context)
        {
            string recruiter1 = "test1@example.com";
            string recruiter2 = "test2@example.com";

            var user1 = new IdentityUser
            {
                UserName = recruiter1,
                NormalizedUserName = recruiter1.ToUpper(),
                Email = recruiter1,
                NormalizedEmail = recruiter1.ToUpper(),
                EmailConfirmed = true
            };

            var user2 = new IdentityUser
            {
                UserName = recruiter2,
                NormalizedUserName = recruiter2.ToUpper(),
                Email = recruiter2,
                NormalizedEmail = recruiter2.ToUpper(),
                EmailConfirmed = true
            };

            PasswordHasher<IdentityUser> passwordHasher = new PasswordHasher<IdentityUser>();
            string passwordHash = passwordHasher.HashPassword(user1, "Test1!");
            user1.PasswordHash = passwordHash;
            user2.PasswordHash = passwordHash;

            context.Users.Add(user1);
            context.Users.Add(user2);
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

                // Read the header.
                string header = reader.ReadLine();

                // Read the rows and create Student entities.

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
                            // Butterfield - 2/24/22
                            // SaveChanges() Error:  Cannot insert explicit value for identity column in table 'Student'
                            // when IDENTITY_INSERT is set to OFF.
                            //
                            // Since the Student:Id field is auto-incremented on the database by configuration
                            // in entity framework, the setting of the Id needs to be ignored.  The Id should
                            // not be present in the CSV file.
                            //
                            //Id = int.Parse(values[0]),

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
