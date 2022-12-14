using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using UniCruiter.Models;
using UniCruiter.Models.Identity;

namespace UniCruiter.Data.SeedData
{
    public static class SeedData
    {
        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new UniCruiterContext(
                serviceProvider.GetRequiredService<
                    DbContextOptions<UniCruiterContext>>()))
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

        private static void SeedUsers(UniCruiterContext context)
        {
            string recruiter1 = "test1@example.com";
            string recruiter2 = "test2@example.com";

            var user1 = new ApplicationUser
            {
                UserName = recruiter1,
                NormalizedUserName = recruiter1.ToUpper(),
                Email = recruiter1,
                NormalizedEmail = recruiter1.ToUpper(),
                EmailConfirmed = true,
                FirstName = "Recruiter1",
                LastName = "Test"
            };

            var user2 = new ApplicationUser
            {
                UserName = recruiter2,
                NormalizedUserName = recruiter2.ToUpper(),
                Email = recruiter2,
                NormalizedEmail = recruiter2.ToUpper(),
                EmailConfirmed = true,
                FirstName = "Recruiter2",
                LastName = "Test"
            };

            PasswordHasher<ApplicationUser> passwordHasher = new PasswordHasher<ApplicationUser>();
            string passwordHash = passwordHasher.HashPassword(user1, "Test1!");
            user1.PasswordHash = passwordHash;
            user2.PasswordHash = passwordHash;

            context.Users.Add(user1);
            context.Users.Add(user2);
        }


        private static void SeedStudents(UniCruiterContext context)
        {
            var assembly = Assembly.GetExecutingAssembly();

            // NOTE:
            // Use the following to get the exact resource name
            // to be assigned to the resourceName variable below.
            string[] resourceNames = assembly.GetManifestResourceNames();

            string resourceName = "UniCruiter.Data.SeedData.Students.csv";
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
                            Season = values[4],
                            Year = int.Parse(values[5]),
                            Email = values[6]
                        }
                    );
                }

                context.SaveChanges();
            }
        }
    }
}
