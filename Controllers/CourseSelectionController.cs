using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using UxtrataTask.Context;
using UxtrataTask.Dtos;
using UxtrataTask.Models;
using UxtrataTask.Repository;

namespace UxtrataTask.Controllers;

public class CourseSelectionController : Controller
{
    private readonly IGenericMySqlAccessRepository<Student> _studentRepo;
    private readonly IGenericMySqlAccessRepository<Course> _courseRepo;
    private readonly IGenericMySqlAccessRepository<CourseSelection> _courseSelectionRepo;
    private readonly IMapper _mapper;

    public CourseSelectionController(
        IGenericMySqlAccessRepository<Student> studentRepo, 
        IGenericMySqlAccessRepository<Course> courseRepo, 
        IGenericMySqlAccessRepository<CourseSelection> courseSelectionRepo, 
        IMapper mapper)
    {
        _studentRepo = studentRepo;
        _courseRepo = courseRepo;
        _courseSelectionRepo = courseSelectionRepo;
        _mapper = mapper;
    }

    // GET: CourseSelection
    public async Task<IActionResult> Index()
    {
        var courseSelections = await _courseSelectionRepo.GetQueryable()
            .Include(cs => cs.Student)
            .Include(cs => cs.Course).ToListAsync();
        return View(courseSelections);
    }

    // GET: CourseSelection/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var courseSelection = await _courseSelectionRepo.GetQueryable()
            .Include(cs => cs.Student)
            .Include(cs => cs.Course)
            .FirstOrDefaultAsync(cs => cs.ID == id);
        if (courseSelection == null)
        {
            return NotFound();
        }

        return View(courseSelection);
    }

    // GET: CourseSelection/Create
    public async Task<IActionResult> Create()
    {
        ViewBag.Students = await _studentRepo.GetAllAsync();
        ViewBag.Courses = await _courseRepo.GetAllAsync();
        return View();
    }

    // POST: CourseSelection/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("StudentID,CourseID,AmountPaid,PaymentDate")] CourseSelectionCreateDto courseSelectionCreateDto)
    {
        var courseSelection = _mapper.Map<CourseSelection>(courseSelectionCreateDto);
        
        if (ModelState.IsValid)
        {
            _courseSelectionRepo.Insert(courseSelection);
            await _courseSelectionRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }
        return View(courseSelection);
    }

    // GET: CourseSelection/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }
        
        ViewBag.Students = await _studentRepo.GetAllAsync();
        ViewBag.Courses = await _courseRepo.GetAllAsync();
        
        var courseSelection = await _courseSelectionRepo.GetAsync(id);
        if (courseSelection == null)
        {
            return NotFound();
        }
        return View(courseSelection);
    }

    // POST: CourseSelection/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("Id,StudentID,CourseID,AmountPaid,PaymentDate")] CourseSelectionEditDto courseSelectionEditDto)
    {
        var courseSelection = _mapper.Map<CourseSelection>(courseSelectionEditDto);
        
        if (id != courseSelection.ID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _courseSelectionRepo.UpdateT(courseSelection);
                await _courseSelectionRepo.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseSelectionExists(courseSelection.ID))
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
        return View(courseSelection);
    }

    // GET: CourseSelection/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var courseSelection = await _courseSelectionRepo.GetQueryable()
            .Include(cs => cs.Student)
            .Include(cs => cs.Course)
            .FirstOrDefaultAsync(cs => cs.ID == id);
        if (courseSelection == null)
        {
            return NotFound();
        }

        return View(courseSelection);
    }
    
    // POST: CourseSelection/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var courseSelection = await _courseSelectionRepo.GetAsync(id);
        _courseSelectionRepo.Delete(courseSelection);
        await _courseSelectionRepo.SaveAsync();
        return RedirectToAction(nameof(Index));
    }
    
    private bool CourseSelectionExists(int id)
    {
        return _courseSelectionRepo.GetQueryable().Any(cs => cs.ID == id);
    }
}