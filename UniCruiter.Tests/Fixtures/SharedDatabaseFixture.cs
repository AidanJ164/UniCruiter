using UniCruiter.Data;
using UniCruiter.Models;
using UniCruiter.Tests.Helpers;
using Microsoft.Data.SqlClient;
using System;
using System.Data.Common;
using System.Linq;
using Microsoft.EntityFrameworkCore;

namespace UniCruiter.Tests.Fixtures
{
    public class SharedDatabaseFixture : IDisposable
    {
        private const string _connectionString = @"Server=(localdb)\mssqllocaldb;Database=UniCruiterTests;Trusted_Connection=True";

        private static readonly object _lock = new();
        private static bool _databaseInitialized;
        public DbConnection Connection { get; }

        public SharedDatabaseFixture()
        {
            Connection = new SqlConnection(_connectionString);

            Seed();

            Connection.Open();
        }

        public void Dispose() => Connection.Dispose();

        public UniCruiterContext CreateContext(DbTransaction transaction = null)
        {
            var context = new UniCruiterContext(
                new DbContextOptionsBuilder<UniCruiterContext>().UseSqlServer(Connection).Options)
;

            if (transaction != null)
            {
                context.Database.UseTransaction(transaction);
            }

            return context;
        }

        private void Seed()
        {
            lock(_lock)
            {
                if(!_databaseInitialized)
                {  
                    using (var context = CreateContext())
                    {
                        context.Database.EnsureDeleted();
                        context.Database.EnsureCreated();

                        addStudents(context);
                        context.SaveChanges();

                        foreach (var student in context.Student.ToArray())
                        {
                            addComment(context, student);
                        }

                        context.SaveChanges();
                    }

                    _databaseInitialized = true;
                }
            }
        }

        private void addStudents(UniCruiterContext context)
        {
            context.Student.AddRange(
                new Student {FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_1, Major = Constants.CSC, Season = Constants.FALL, Year = Constants.GRAD2022},
                new Student {FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_2, Major = Constants.CENG, Season = Constants.SPRING, Year = Constants.GRAD2023},
                new Student {FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_3, Major = Constants.CSC, Season = Constants.FALL, Year = Constants.GRAD2022},
                new Student {FirstName = Constants.FIRST_NAME, LastName = Constants.LAST_NAME_4, Major = Constants.CENG, Season = Constants.SPRING, Year = Constants.GRAD2023}
                );
        }

        private void addComment (UniCruiterContext context, Student student)
        {
            context.Comment
                .Add(
                    new Comment
                    {
                        EnteredOn = DateTime.Now,
                        Text = Constants.COMMENT_TEXT,
                        Student = student,
                        ApplicationUser = context.Users.FirstOrDefault()
                    });
        }

    }
}
