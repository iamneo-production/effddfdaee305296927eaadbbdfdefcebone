using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreDBFirst.Models;

namespace BookStoreDBFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CourseController : ControllerBase
    {
        private readonly CourseEnquiryDbContext _context;

        public CourseController(CourseEnquiryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Course>>> GetAllCourses()
        {
            var courses = await _context.Courses.ToListAsync();
            return Ok(courses);
        }
[HttpGet("CourseName")]
public async Task<ActionResult<IEnumerable<string>>> Get()
{
    // Project the CourseTitle property using Select
    var CourseTitles = await _context.Courses
        .OrderBy(x => x.CourseName)
        .Select(x => x.CourseName)
        .ToListAsync();

    return CourseTitles;
}
        [HttpPost]
        public async Task<ActionResult> AddCourse(Course course)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return detailed validation errors
            }
            await _context.Courses.AddAsync(course);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCourse(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Course id");

            var course = await _context.Courses.FindAsync(id);
              _context.Courses.Remove(course);
                await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
