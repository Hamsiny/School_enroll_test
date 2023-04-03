using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UxtrataTask.Dtos;
using UxtrataTask.Models;
using UxtrataTask.Repository;

namespace UxtrataTask.Controllers;

public class CourseController : Controller
{
    private readonly IGenericMySqlAccessRepository<Course> _courseRepo;
    private readonly IMapper _mapper;

    public CourseController(IGenericMySqlAccessRepository<Course> courseRepo, IMapper mapper)
    {
        _courseRepo = courseRepo;
        _mapper = mapper;
    }

    // GET: Course
    public async Task<IActionResult> Index()
    {
        return View(await _courseRepo.GetAllAsync());
    }

    // GET: Course/Details/5
    public async Task<IActionResult> Details(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _courseRepo.GetQueryable()
            .FirstOrDefaultAsync(c => c.CourseID == id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // GET: Course/Create
    public IActionResult Create()
    {
        return View();
    }
    
    // POST: Course/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create([Bind("CourseName,Cost")] CourseCreateDto courseCreateDto)
    {
        var course = _mapper.Map<Course>(courseCreateDto);
        
        if (ModelState.IsValid)
        {
            _courseRepo.Insert(course);
            await _courseRepo.SaveAsync();
            return RedirectToAction(nameof(Index));
        }

        return View(course);

    }

    // GET: Course/Edit/5
    public async Task<IActionResult> Edit(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _courseRepo.GetAsync(id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST: Course/Edit/5
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, [Bind("CourseID,CourseName,Cost")] CourseEditDto courseEditDto)
    {
        var course = _mapper.Map<Course>(courseEditDto);
        
        if (id != course.CourseID)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            try
            {
                _courseRepo.UpdateT(course);
                await _courseRepo.SaveAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CourseExists(course.CourseID))
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

        return View(course);
    }

    // GET: Course/Delete/5
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var course = await _courseRepo.GetQueryable()
            .FirstOrDefaultAsync(c => c.CourseID == id);
        if (course == null)
        {
            return NotFound();
        }

        return View(course);
    }

    // POST: Course/Delete/5
    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var course = await _courseRepo.GetAsync(id);
        _courseRepo.Delete(course);
        await _courseRepo.SaveAsync();
        return RedirectToAction(nameof(Index));
    }

    private bool CourseExists(int id)
    {
        return _courseRepo.GetQueryable().Any(c => c.CourseID == id);
    }
}