using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UxtrataTask.Models;
using UxtrataTask.Dtos;
using UxtrataTask.Repository;

namespace UxtrataTask.Controllers;

public class StudentController : Controller
{
    private readonly IGenericMySqlAccessRepository<Student> _studentRepo;
    private readonly IMapper _mapper;

    public StudentController(IGenericMySqlAccessRepository<Student> studentRepo, IMapper mapper)
    {
        _studentRepo = studentRepo;
        _mapper = mapper;
    }

    // GET: Student
    public async Task<IActionResult> Index()
    {
        return View(await _studentRepo.GetAllAsync());
    }

    // GET: Student/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _studentRepo.GetQueryable()
            .FirstOrDefaultAsync(s => s.StudentID == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // GET: Student/Create
    public IActionResult Create()
    {
        return View();
    }

    // POST: Student/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("Name,Age")] StudentCreateDto studentCreateDto)
    {
        var student = _mapper.Map<Student>(studentCreateDto);
        
        if (ModelState.IsValid)
        {
            _studentRepo.Insert(student);
            await _studentRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(student);
    }

    // GET: Student/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _studentRepo.GetAsync(id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: Student/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("StudentID,Name,Age")] StudentEditDto studentEditDto)
    {
        var student = _mapper.Map<Student>(studentEditDto);
        
        if (id != student.StudentID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _studentRepo.UpdateT(student);
                await _studentRepo.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!StudentExists(student.StudentID))
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

    // GET: Student/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var student = await _studentRepo.GetQueryable()
            .FirstOrDefaultAsync(s => s.StudentID == id);
        if (student == null)
        {
            return NotFound();
        }

        return View(student);
    }

    // POST: Student/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var student = await _studentRepo.GetAsync(id);
        _studentRepo.Delete(student);
        await _studentRepo.SaveAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool StudentExists(int id)
    {
        return _studentRepo.GetQueryable().Any(s => s.StudentID == id);
    }
}