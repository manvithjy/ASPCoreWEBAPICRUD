using ASPCoreWEBAPICRUD.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace ASPCoreWEBAPICRUD.Controllers

//database first approach

/// API Controller for managing Student entities.
/// Provides CRUD operations using a database-first approach.
{
    [Route("api/[controller]")]
    [ApiController]
    public class StudentAPIController : ControllerBase
    {
        private readonly MyDbContext context;

        public ILogger<StudentAPIController> _logger;

        public StudentAPIController(MyDbContext context, ILogger<StudentAPIController> logger)
        {
            this.context = context;
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<List<Student>>> GetStudents()
        {
            _logger.LogInformation("fetching all students");
            var data = await context.Students.ToListAsync();
            _logger.LogInformation("fetched {Count} students", data.Count);
            return Ok(data);

        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Student>> GetStudentById(int id)
        {
            var student = await context.Students.FindAsync(id);
            if (student == null)
            {
                return NotFound();
            }
            return Ok(student);
        }

        [HttpPost]
        public async Task<ActionResult<Student>> CreateStudent(Student std)
        {
            await context.Students.AddAsync(std);
            await context.SaveChangesAsync();
            return Ok(std);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult<Student>> UpdateStudent(int id, Student std)
        {
            if (id != std.StudentId)
            {
                return BadRequest();
            }
            context.Entry(std).State = EntityState.Modified;
            await context.SaveChangesAsync();
            return Ok(std);

        }

        [HttpDelete("{id}")]
        public async Task<ActionResult<Student>> DeleteStudent(int id)
        {
            var std = await context.Students.FindAsync(id); // Await the ValueTask to get the actual Student object
            if (std == null)
            {
                return NotFound();
            }
            context.Students.Remove(std); // Pass the resolved Student object to Remove
            await context.SaveChangesAsync();
            return Ok();
        }
    }
}