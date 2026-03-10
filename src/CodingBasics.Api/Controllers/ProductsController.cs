using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using CodingBasics.Infrastructure.Persistence; // Ensure this matches the file's namespace

namespace CodingBasics.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProductsController : ControllerBase
{
    private readonly AdventureWorksDbContext _context;

    public ProductsController(AdventureWorksDbContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetProducts()
    {
        // This provides the data for Item 1 (Total Value) and Item 2 (Colors)
        var products = await _context.Products.ToListAsync();
        return Ok(products);
    }
}