using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using StuManSys.Data;
using StudentManagementSys.Model;
using StuManSys.Services;
using Microsoft.DotNet.Scaffolding.Shared.ProjectModel;

namespace StuManSys.Controllers
{
    public class StudentsController : Controller
    {
        private readonly StuManSysContext _context;
        private readonly StudentServices _StuService;
        public StudentsController(StuManSysContext context)
        {
            _context = context;
            _StuService = new StudentServices(context);
        }

        // GET: Students
        public async Task<IActionResult> Index()
        {
              return _context.Student != null ? 
                          View(await _context.Student.ToListAsync()) :
                          Problem("Entity set 'StuManSysContext.Student'  is null.");
        }

        // GET: Students/Details/5
        public async Task<IActionResult> Details(string id)
        {
            //String Name = HttpContext.User.Identity.Name;
            //if (id == null || _context.Student == null)
            //{
            //    return NotFound();
            //}

            //var student = await _context.Student
            //    .FirstOrDefaultAsync(m => m.UID == id);
            //if (student == null)
            //{
            //    return NotFound();
            //}
            var student = _StuService.GetStudent(id).Result;

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
        public async Task<IActionResult> Create([Bind("SchoolSession,CPA,TotalCredit,PassedCredit,ClassRoomID,Program,SubjectEnlisted,UID,Name,Status,BirtDate,Type,PhoneNumber,Email,Sex,Address,Relative,YearofStart,Religion,Authority,BCKey,StoreID")] Student student)
        {
            //if (ModelState.IsValid)
            {
                _context.Add(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            //return View(student);
        }

        // GET: Students/Edit/5
        public async Task<IActionResult> Edit(string id)
        {
            if (id == null || _context.Student == null)
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
        public async Task<IActionResult> Edit(string id, [Bind("SchoolSession,CPA,TotalCredit,PassedCredit,ClassRoomID,Program,SubjectEnlisted,UID,Name,Status,BirtDate,Type,PhoneNumber,Email,Sex,Address,Relative,YearofStart,Religion,Authority,BCKey,StoreID")] Student student)
        {
            if (id != student.UID)
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
                    if (!StudentExists(student.UID))
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
        public async Task<IActionResult> Delete(string id)
        {
            if (id == null || _context.Student == null)
            {
                return NotFound();
            }

            var student = await _context.Student
                .FirstOrDefaultAsync(m => m.UID == id);
            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(string id)
        {
            if (_context.Student == null)
            {
                return Problem("Entity set 'StuManSysContext.Student'  is null.");
            }
            var student = await _context.Student.FindAsync(id);
            if (student != null)
            {
                _context.Student.Remove(student);
            }
            
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool StudentExists(string id)
        {
          return (_context.Student?.Any(e => e.UID == id)).GetValueOrDefault();
        }
    }
}
