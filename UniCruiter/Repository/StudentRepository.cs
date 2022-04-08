using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UniCruiter.Data;
using UniCruiter.Models;
using UniCruiter.ViewModels;

namespace UniCruiter.Repository
{
    public class StudentRepository : IStudentRepository, IDisposable
    {
        private UniCruiterContext _context;

        public StudentRepository(UniCruiterContext context)
        {
            _context = context;
        }

        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        protected virtual void Dispose(bool disposing)
        {
            //Cleanup created context
            _context = null;
        }


        public async Task<Student> GetStudentByID(int studentId)
        {
            return await _context.Student.FirstOrDefaultAsync(s => s.Id == studentId);
        }

        public async Task<IList<Student>> GetStudents(StudentViewModel studentViewModel)
        {
            IEnumerable<string> majorQuery = from m in _context.Student orderby m.Major ascending select m.Major;
            IEnumerable<int> yearQuery = from y in _context.Student orderby y.Year ascending select y.Year;
            IEnumerable<string> seasonQuery = from s in _context.Student
                                             orderby s.Season == "Winter", s.Season == "Fall", s.Season == "Summer", s.Season == "Spring"
                                             select s.Season;

            var students = from s in _context.Student
                           select s;

            //Filter by Name
            if (!string.IsNullOrEmpty(studentViewModel.FirstName))
            {
                students = students.Where(s =>
                    s.FirstName.StartsWith(studentViewModel.FirstName)
                );
            }
            if (!string.IsNullOrEmpty(studentViewModel.LastName))
            {
                students = students.Where(s =>
                    s.LastName.StartsWith(studentViewModel.LastName)
                );
            }

            //Filter by Major
            if (!string.IsNullOrEmpty(studentViewModel.Major))
            {
                students = students.Where(s => s.Major == studentViewModel.Major);
            }

            //Filter by Grad Season
            if (!string.IsNullOrEmpty(studentViewModel.Season))
            {
                students = students.Where(s => s.Season == studentViewModel.Season);
            }
            
            //Filter by Grad Year
            if (studentViewModel.Year != 0)
            {
                students = students.Where(s => s.Year == studentViewModel.Year);
            }

            students = students.OrderBy(s => s.LastName);

            studentViewModel.Majors = new SelectList(majorQuery.Distinct());
            studentViewModel.Seasons = new SelectList(seasonQuery.Distinct());
            studentViewModel.Years = new SelectList(yearQuery.Distinct());

            return await students.ToListAsync();
        }

        public async Task<Student> InsertStudent(StudentViewModel studentViewModel)
        {
            Student student = new()
            {
                FirstName = studentViewModel.FirstName.Trim(),
                LastName = studentViewModel.LastName.Trim(),
                Major = studentViewModel.Major,
                Season = studentViewModel.Season,
                Year = studentViewModel.Year,
                Email = studentViewModel.Email,
        };

            _context.Add(student);
            await _context.SaveChangesAsync();

            return student;

        }

        public async Task<Student> UpdateStudent(StudentViewModel studentViewModel)
        {
            Student student;

            try
            {
                student = await _context.Student.FindAsync(studentViewModel.Id);

                student.FirstName = studentViewModel.FirstName.Trim();
                student.LastName = studentViewModel.LastName.Trim();
                student.Major = studentViewModel.Major;
                student.Season = studentViewModel.Season;
                student.Year = studentViewModel.Year;
                student.Email = studentViewModel.Email;

                _context.Update(student);
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(studentViewModel.Id))
                {
                    return null;
                }
                else
                {
                    throw;
                }
            }

            return student;

        }

        public async Task DeleteStudent(int studentID)
        {

            var student = _context.Student.FirstOrDefault(s => s.Id == studentID);

            _context.Student.Remove(student);

            await _context.SaveChangesAsync();

        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(s => s.Id == id);
        }
    }
}