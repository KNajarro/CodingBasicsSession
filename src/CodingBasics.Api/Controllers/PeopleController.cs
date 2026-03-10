using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodingBasics.Infrastructure.Persistence;

namespace CodingBasics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class PeopleController : ControllerBase
{
    private readonly AdventureWorksDbContext _context;

    public PeopleController(AdventureWorksDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetPeople()
    {
        // This provides the data for Item 3 (Person Types)
        var people = await _context.People.ToListAsync();
        return Ok(people);
    }
}