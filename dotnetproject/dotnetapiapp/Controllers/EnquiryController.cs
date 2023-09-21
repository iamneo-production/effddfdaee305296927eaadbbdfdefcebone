using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Threading.Tasks;
using BookStoreDBFirst.Models;

namespace BookStoreDBFirst.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class EnquiryController : ControllerBase
    {
        private readonly CourseEnquiryDbContext _context;

        public EnquiryController(CourseEnquiryDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Enquiry>>> GetAllEnquiries()
        {
            var enquirys = await _context.Enquires.ToListAsync();
            return Ok(enquirys);
        }

        [HttpPost]
        public async Task<ActionResult> AddEnquiry(Enquiry enquiry)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState); // Return detailed validation errors
            }
            await _context.Enquires.AddAsync(enquiry);
            await _context.SaveChangesAsync();
            return Ok();
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEnquiry(int id)
        {
            if (id <= 0)
                return BadRequest("Not a valid Enquiry id");

            var enquiry = await _context.Enquires.FindAsync(id);
              _context.Enquires.Remove(enquiry);
                await _context.SaveChangesAsync();
            return NoContent();
        }
    }
}
