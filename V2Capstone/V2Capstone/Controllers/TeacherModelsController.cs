using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using V2Capstone.Data;
using V2Capstone.Models;

namespace V2Capstone.Controllers
{
    public class TeacherModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public TeacherModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: TeacherModels
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.Teacher.Include(t => t.User);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: TeacherModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherModel = await _context.Teacher
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacherModel == null)
            {
                return NotFound();
            }

            return View(teacherModel);
        }

        // GET: TeacherModels/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: TeacherModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("TeacherId,FirstName,LastName,Id")] TeacherModel teacherModel)
        {
            if (ModelState.IsValid)
            {
                _context.Add(teacherModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", teacherModel.Id);
            return View(teacherModel);
        }

        // GET: TeacherModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherModel = await _context.Teacher.FindAsync(id);
            if (teacherModel == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", teacherModel.Id);
            return View(teacherModel);
        }

        // POST: TeacherModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("TeacherId,FirstName,LastName,Id")] TeacherModel teacherModel)
        {
            if (id != teacherModel.TeacherId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(teacherModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!TeacherModelExists(teacherModel.TeacherId))
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
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", teacherModel.Id);
            return View(teacherModel);
        }

        // GET: TeacherModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var teacherModel = await _context.Teacher
                .Include(t => t.User)
                .FirstOrDefaultAsync(m => m.TeacherId == id);
            if (teacherModel == null)
            {
                return NotFound();
            }

            return View(teacherModel);
        }

        // POST: TeacherModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var teacherModel = await _context.Teacher.FindAsync(id);
            _context.Teacher.Remove(teacherModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool TeacherModelExists(int id)
        {
            return _context.Teacher.Any(e => e.TeacherId == id);
        }

        //public IActionResult GetTeacherStudents()
        //{
        //    AnalyticsViewModel viewModel = new AnalyticsViewModel();
        //    string teacherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
        //    var teacher = _context.Teacher.Where(t => t.Id == teacherId).FirstOrDefault();
        //    viewModel.Students = _context.Student.Where(s => s.TeacherId == teacher.TeacherId).ToList();
        //    viewModel.Students = viewModel.Students.Where(s => s.IsNotified == !false).ToList();
        //    viewModel.Parents = _context.Parent.Where(p => viewModel.Students.Any(s => s.ParentId == p.ParentId)).ToList();
        //    viewModel.Parents = viewModel.Parents.Where(p => p.IsNotified == !false).ToList();

        //    return RedirectToAction("GetParents", viewModel);
        //}

        //public IActionResult GetParents(AnalyticsViewModel viewModel)
        //{
        //    viewModel.Students = viewModel.Students.Where(s => s.IsNotified == !false).ToList();
        //    viewModel.Parents = _context.Parent.Where(p => viewModel.Students.Any(s => s.ParentId == p.ParentId)).ToList();
        //    return RedirectToAction("ChosenParents", viewModel);
        //}

        public IActionResult ChosenParents()
        {
            AnalyticsViewModel viewModel = new AnalyticsViewModel();
            string teacherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var teacher = _context.Teacher.Where(t => t.Id == teacherId).FirstOrDefault();
            viewModel.Students = _context.Student.Where(s => s.TeacherId == teacher.TeacherId).ToList();
            viewModel.Students = viewModel.Students.Where(s => s.IsNotified == !false).ToList();
            viewModel.Parents = _context.Parent.Where(p => viewModel.Students.Any(s => s.ParentId == p.ParentId)).ToList();
            viewModel.Parents = viewModel.Parents.Where(p => p.IsNotified == !false).ToList();
            return View(viewModel);
        }

        public async Task<IActionResult> SendNotification(AnalyticsViewModel viewModel)
        {
            string status = "All";
            string teacherId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            TeacherModel teacher = _context.Teacher.Where(e => e.Id == teacherId).Single();
            viewModel.alert.TeacherId = teacher.TeacherId;
            viewModel.alert.Recipients = status;
            _context.Alert.Add(viewModel.alert);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }
    }
}
