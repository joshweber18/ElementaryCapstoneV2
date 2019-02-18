using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using DevExtreme.AspNet.Data;
using DevExtreme.AspNet.Mvc;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Newtonsoft.Json;
using V2Capstone.Data;
using V2Capstone.Models;

namespace V2Capstone.Controllers
{
    public class StudentModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public object Get(DataSourceLoadOptions loadOptions)
        {
            return DataSourceLoader.Load(_context.Student, loadOptions);
        }

        public IActionResult Save(int key, string values)
        {
            var student = _context.Student.First(a => a.StudentId == key);
            JsonConvert.PopulateObject(values, student);

            if (!TryValidateModel(student))
            {
                return NotFound();
            }

            _context.SaveChanges();

            return Ok();
        }

        // GET: StudentModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Student.Include(s => s.Parent).Include(s => s.Teacher).Include(s => s.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: StudentModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.Student
                .Include(s => s.Parent)
                .Include(s => s.Teacher)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentModel == null)
            {
                return NotFound();
            }

            return View(studentModel);
        }

        // GET: StudentModels/Create
        public IActionResult Create()
        {
            ViewData["ParentId"] = new SelectList(_context.Parent, "ParentId", "ParentId");
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId");
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: StudentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("StudentId,FirstName,LastName,ScienceGrade,MathGrade,HistoryGrade,TeacherId,ParentId,Id")] StudentModel studentModel)
        {
            if (ModelState.IsValid)
            {
                string studentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                studentModel.Id = studentId;
                _context.Add(studentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["ParentId"] = new SelectList(_context.Parent, "ParentId", "ParentId", studentModel.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", studentModel.TeacherId);
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", studentModel.Id);
            return View(studentModel);
        }

        // GET: StudentModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.Student.FindAsync(id);
            if (studentModel == null)
            {
                return NotFound();
            }
            ViewData["ParentId"] = new SelectList(_context.Parent, "ParentId", "ParentId", studentModel.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", studentModel.TeacherId);
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", studentModel.Id);
            return View(studentModel);
        }

        // POST: StudentModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("StudentId,FirstName,LastName,ScienceGrade,MathGrade,HistoryGrade,GradeLevel,TeacherId,ParentId,Id")] StudentModel studentModel)
        {
            studentModel = _context.Student.Where(s => s.StudentId == id).FirstOrDefault();
            if (id != studentModel.StudentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(studentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!StudentModelExists(studentModel.StudentId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction("GetStudents", "TeacherModels");
            }
            ViewData["ParentId"] = new SelectList(_context.Parent, "ParentId", "ParentId", studentModel.ParentId);
            ViewData["TeacherId"] = new SelectList(_context.Teacher, "TeacherId", "TeacherId", studentModel.TeacherId);
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", studentModel.Id);
            return RedirectToAction("GetStudents");
        }

        // GET: StudentModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var studentModel = await _context.Student
                .Include(s => s.Parent)
                .Include(s => s.Teacher)
                .Include(s => s.User)
                .FirstOrDefaultAsync(m => m.StudentId == id);
            if (studentModel == null)
            {
                return NotFound();
            }

            return View(studentModel);
        }

        // POST: StudentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var studentModel = await _context.Student.FindAsync(id);
            _context.Student.Remove(studentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentModelExists(int id)
        {
            return _context.Student.Any(e => e.StudentId == id);
        }
    }
}
