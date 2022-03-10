using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UniCruiter.Data;
using UniCruiter.Models;
using UniCruiter.ViewModels;

namespace UniCruiter.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly UniCruiterContext _context;

        public StudentsController(UniCruiterContext context)
        {
            _context = context;
        }

        // GET: Students
        public async Task<IActionResult> Index(string searchFirst, string searchLast, string sortOrder, string major, string season, string year)
        {
            ViewData["SeasonSortParam"] = sortOrder == "Season" ? "season_desc" : "Season";
            ViewData["YearSortParam"] = sortOrder == "Year" ? "year_desc" : "Year";
            ViewData["MajorSortParam"] = sortOrder == "Major" ? "major_desc" : "Major";

            IQueryable<string> majorQuery = from m in _context.Student orderby m.Major select m.Major;
            IQueryable<int> yearQuery = from y in _context.Student orderby y.Year select y.Year;
            IQueryable<string> seasonQuery = from s in _context.Student orderby s.Season select s.Season;

            var students = from s in _context.Student
                           select s;

            if (!string.IsNullOrEmpty(searchFirst))
            {
                students = students.Where(f => f.FirstName.Trim().StartsWith(searchFirst));
            }

            if (!string.IsNullOrEmpty(searchLast))
            {
                students = students.Where(l => l.LastName.Trim().StartsWith(searchLast));
            }

            if(!string.IsNullOrEmpty(major))
            {
                students = students.Where(s => s.Major == major);
            }

            if (!string.IsNullOrEmpty(season))
            {
                students = students.Where(s => s.Season == season);
            }

            if (!string.IsNullOrEmpty(year))
            {
                students = students.Where(s => s.Year.ToString() == year);
            }

            switch (sortOrder)
            {
                case "Season":
                    students = students.OrderBy(s => s.Season);
                    break;
                case "season_desc":
                    students = students.OrderByDescending(s => s.Season);
                    break;
                case "Year":
                    students = students.OrderBy(s => s.Year);
                    break;
                case "year_desc":
                    students = students.OrderByDescending(s => s.Year);
                    break;
                case "Major":
                    students = students.OrderBy(s => s.Major);
                    break;
                case "major_desc":
                    students = students.OrderByDescending(s => s.Major);
                    break;
                default:
                    students = students.OrderBy(s => s.LastName);
                    break;
            }

            var studentVM = new StudentSearchViewModel
            {
                Majors = new SelectList(await majorQuery.Distinct().ToListAsync()),
                Seasons = new SelectList(await seasonQuery.Distinct().ToListAsync()),
                Years = new SelectList(await yearQuery.Distinct().ToListAsync()),
                Students = await students.AsNoTracking().ToListAsync()
            };

            return View(studentVM);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            return View();
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Major,Season,Year")] Student student)
        {
            if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return View(student);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Major,Season,Year")] Student student)
        {
            if (id != student.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(student);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentExists(student.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            return View(student);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Student.FindAsync(id);
            _context.Student.Remove(student);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(int id)
        {
            return _context.Student.Any(e => e.Id == id);
        }
    }
}
