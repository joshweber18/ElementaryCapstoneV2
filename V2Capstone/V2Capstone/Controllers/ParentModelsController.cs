using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using V2Capstone.Data;
using V2Capstone.Models;
using Microsoft.AspNetCore.Server.Kestrel.Core.Internal.Http;

namespace V2Capstone.Controllers
{
    public class ParentModelsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public ParentModelsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: ParentModels
        public IActionResult Index()
        {
            AnalyticsViewModel viewModel = new AnalyticsViewModel();
            viewModel.Alerts = _context.Alert.Where(a => a.Recipients == "All").ToList();
            string parentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            viewModel.parent = _context.Parent.Where(p => p.Id == parentId).Single();
            var applicationDbContext = _context.Parent.Include(p => p.User);

            //var parent = _context.Parent.Where(p => p.Id == parentId).FirstOrDefault();
            //viewModel.Students = _context.Student.Where(s => s.ParentId == parent.ParentId).ToList();

            //foreach (var student in viewModel.Students)
            //{
            //    if (student.UpdatedGrade == true)
            //    {
                    
            //    }
            //}

            return View(viewModel);
        }

        // GET: ParentModels/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentModel = await _context.Parent
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ParentId == id);
            if (parentModel == null)
            {
                return NotFound();
            }

            return View(parentModel);
        }

        // GET: ParentModels/Create
        public IActionResult Create()
        {
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id");
            return View();
        }

        // POST: ParentModels/Create
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ParentId,FirstName,LastName,Id")] ParentModel parentModel)
        {
            if (ModelState.IsValid)
            {
                string parentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
                parentModel.Id = parentId;
                _context.Add(parentModel);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", parentModel.Id);
            return View(parentModel);
        }

        // GET: ParentModels/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentModel = await _context.Parent.FindAsync(id);
            if (parentModel == null)
            {
                return NotFound();
            }
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", parentModel.Id);
            return View(parentModel);
        }

        // POST: ParentModels/Edit/5
        // To protect from overposting attacks, please enable the specific properties you want to bind to, for 
        // more details see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ParentId,FirstName,LastName,Id")] ParentModel parentModel)
        {
            if (id != parentModel.ParentId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(parentModel);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ParentModelExists(parentModel.ParentId))
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
            ViewData["Id"] = new SelectList(_context.Set<ApplicationUser>(), "Id", "Id", parentModel.Id);
            return View(parentModel);
        }

        // GET: ParentModels/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var parentModel = await _context.Parent
                .Include(p => p.User)
                .FirstOrDefaultAsync(m => m.ParentId == id);
            if (parentModel == null)
            {
                return NotFound();
            }

            return View(parentModel);
        }

        // POST: ParentModels/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var parentModel = await _context.Parent.FindAsync(id);
            _context.Parent.Remove(parentModel);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool ParentModelExists(int id)
        {
            return _context.Parent.Any(e => e.ParentId == id);
        }


        public async Task<IActionResult> CompleteGrade(string id)
        {
            AnalyticsViewModel viewModel = new AnalyticsViewModel();
            string parentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parent = _context.Parent.Where(p => p.Id == parentId).FirstOrDefault();
            List<StudentModel> parentsStudent = _context.Student.Where(s => s.ParentId == parent.ParentId).ToList();
            List<StudentModel> allStudents = _context.Student.OrderByDescending(s => s.HistoryGrade).ToList();
            List<StudentModel> rankedStudent = allStudents.Where(s => parentsStudent.Any(p => p.Id == s.Id)).ToList();
            viewModel.student = rankedStudent.Find(r => r.Id == id);

            List<StudentModel> historyStudents = _context.Student.OrderByDescending(s => s.HistoryGrade).ToList();
            viewModel.student.HistoryRank = historyStudents.FindIndex(s => parentsStudent.Any(p => p.Id == s.Id)) + 1;

            List<StudentModel> mathStudents = _context.Student.OrderByDescending(s => s.MathGrade).ToList();
            viewModel.student.MathRank = mathStudents.FindIndex(s => parentsStudent.Any(p => p.Id == s.Id)) + 1;

            List<StudentModel> scienceStudents = _context.Student.OrderByDescending(s => s.ScienceGrade).ToList();
            viewModel.student.ScienceRank = scienceStudents.FindIndex(s => parentsStudent.Any(p => p.Id == s.Id)) + 1;

            List<StudentModel> everyStudent = _context.Student.Select(reg => reg).ToList();
            foreach (var students in allStudents)
            {
                students.OverallGrade = (students.MathGrade + students.ScienceGrade + students.HistoryGrade) / 3;
            }
            List<StudentModel> studentGrades = everyStudent.OrderByDescending(s => s.OverallGrade).ToList();
            viewModel.student.OverallRank = studentGrades.FindIndex(s => parentsStudent.Any(p => p.Id == s.Id)) + 1;
            _context.Update(viewModel.student);
            await _context.SaveChangesAsync();
            return View(viewModel);
        }

        public IActionResult ParentsChildrenList()
        {
            AnalyticsViewModel viewModel = new AnalyticsViewModel();
            string parentId = this.User.FindFirstValue(ClaimTypes.NameIdentifier);
            var parent = _context.Parent.Where(p => p.Id == parentId).FirstOrDefault();
            viewModel.Students = _context.Student.Where(s => s.ParentId == parent.ParentId).ToList();
            return View(viewModel);
        }

        public string TestMethod()
        {
            return "Test String";
        }
    }
}
