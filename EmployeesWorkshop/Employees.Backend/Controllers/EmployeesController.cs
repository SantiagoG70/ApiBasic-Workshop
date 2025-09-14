using DocuSign.eSign.Model;
using Employees.Backend.Data;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Controllers;

[ApiController]
[Route("api/[Controller]")]
public class EmployeesController : ControllerBase
{
    private readonly DataContext _context;

    public EmployeesController(DataContext context)
    {
        _context = context;
    }

    [HttpGet]
    public async Task<IActionResult> GetAsync()
    {
        return Ok(await _context.Employees.ToListAsync());
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetAsync(int id)
    {
        var country = await _context.Employees.FirstOrDefaultAsync(c => c.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        return Ok(country);
    }

    [HttpGet("search/{text}")]
    public async Task<IActionResult> GetAsync(string text)
    {
        var employees = await _context.Employees
            .Where(e => e.FirstName.Contains(text) || e.LastName.Contains(text))
            .ToListAsync();

        return Ok(employees);
    }

    [HttpPost]
    public async Task<IActionResult> PostAsync(Employee employee)
    {
        _context.Employees.Add(employee);
        await _context.SaveChangesAsync();
        return Ok(employee);
    }

    [HttpDelete("{id}")]
    public async Task<IActionResult> DeleteAsync(int id)
    {
        var country = await _context.Employees.FirstOrDefaultAsync(c => c.Id == id);
        if (country == null)
        {
            return NotFound();
        }

        _context.Remove(country);
        await _context.SaveChangesAsync();
        return NoContent();
    }

    [HttpPut]
    public async Task<IActionResult> PutAsync(Employee employee)
    {
        _context.Update(employee);
        await _context.SaveChangesAsync();
        return Ok(employee);
    }
}