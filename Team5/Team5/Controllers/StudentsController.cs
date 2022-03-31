using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using UniCruiter.Repository;
using UniCruiter.ViewModels;

namespace UniCruiter.Controllers
{
    [Authorize]
    public class StudentsController : Controller
    {
        private readonly IStudentRepository _studentRepository;

        public StudentsController(IStudentRepository studentRepository)
        {
            _studentRepository = studentRepository;
        }

        // GET: Students
        public async Task<IActionResult> Index(StudentViewModel studentViewModel)
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
            var student = new Student()
            {
                Id = studentVM.Id,
                FirstName = studentVM.FirstName,
                LastName = studentVM.LastName,
                Major = studentVM.Major,
                Season = studentVM.Season,
                Year = studentVM.Year,
                Email = studentVM.Email
            };

            if (ModelState.IsValid)
            {
                await _studentRepository.InsertStudent(studentViewModel);
                return RedirectToAction(nameof(Index));
            }

            return View();
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

            var student = new Student
            {
                Id = studentVM.Id,
                FirstName = studentVM.FirstName,
                LastName = studentVM.LastName,
                Major = studentVM.Major,
                Season = studentVM.Season,
                Year = studentVM.Year,
                Email = studentVM.Email
            };

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
