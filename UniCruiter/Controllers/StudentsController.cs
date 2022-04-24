using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;
using UniCruiter.Models.Identity;
using UniCruiter.Repository;
using UniCruiter.ViewModels;

namespace UniCruiter.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;
        private readonly UserManager<ApplicationUser> _userManager;

        public StudentsController(IStudentRepository studentRepository, UserManager<ApplicationUser> userManager)
        {
            _studentRepository = studentRepository;
            _userManager = userManager;
        }

        // GET: Students
        public async Task<IActionResult> Index(StudentViewModel studentViewModel)
        {
            studentViewModel.Students = await _studentRepository.GetStudents(studentViewModel);

            return View(studentViewModel);
        }

        public async Task<IActionResult> CardIndex(StudentViewModel studentViewModel)
        {
            studentViewModel.Students = await _studentRepository.GetStudents(studentViewModel);

            return View(studentViewModel);
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentViewModel studentViewModel = new(await _studentRepository.GetStudentByID((int)id));

            if (studentViewModel.Id == 0)
            {
                return NotFound();
            }

            return View(studentViewModel);
        }

        // GET: Students/Create
        public IActionResult Create()
        {
            StudentViewModel studentViewModel = new();

            return View(studentViewModel);
        }

        // POST: Students/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("Id,FirstName,LastName,Major,Season,Year,Email")] StudentViewModel studentViewModel)
        {
            if (ModelState.IsValid)
            {
                await _studentRepository.InsertStudent(studentViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View();
        }

        // GET: Student/CreateComment
        public async Task<IActionResult> CreateComment(int Id)
        {
            StudentViewModel studentViewModel = new(await _studentRepository.GetStudentByID(Id));

            return View(studentViewModel);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> CreateComment([Bind("Id,FirstName,LastName,CommentText")] StudentViewModel studentViewModel)
        {
            if (studentViewModel.CommentText != null)
            {
                studentViewModel.CommentEnteredOn = DateTime.Now;
                studentViewModel.CommentEnteredBy = await _userManager.GetUserAsync(User);
                await _studentRepository.InsertComment(studentViewModel);
            }
            return RedirectToAction(nameof(Details), new { studentViewModel.Id });
        }

        // GET: Student/DeleteComment/5
        public async Task<IActionResult> DeleteComment(int id, int studentId)
        {
            var Id = studentId;

            await _studentRepository.DeleteComment(id);
            return RedirectToAction(nameof(Details), new { Id });
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentViewModel studentViewModel = new(await _studentRepository.GetStudentByID((int)id));

            if (studentViewModel.Id == 0)
            {
                return NotFound();
            }
            return View(studentViewModel);
        }

        // POST: Students/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,FirstName,LastName,Major,Season,Year,Email")]
                                              StudentViewModel studentViewModel)
        {
            if (id != studentViewModel.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                var updatedStudent = await _studentRepository.UpdateStudent(studentViewModel);
                if (updatedStudent == null)
                {
                    return NotFound();
                }

                return RedirectToAction(nameof(Index));
            }

            return View(studentViewModel);
        }

        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            StudentViewModel studentViewModel = new(await _studentRepository.GetStudentByID((int)id));

            if (studentViewModel.Id == 0)
            {
                return NotFound();
            }

            return View(studentViewModel);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            await _studentRepository.DeleteStudent(id);

            return RedirectToAction(nameof(Index));
        }
    }
}
