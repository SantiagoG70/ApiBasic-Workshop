using Employees.Backend.Data;
using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Orders.Backend.Controllers;

namespace Employees.Backend.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EmployeesController : GenericController<Employee>
{
    private readonly DataContext _context;

    public EmployeesController(
        DataContext context,
        IGenericUnitOfWork<Employee> unit) : base(unit)
    {
        _context = context;
    }

    [HttpGet("search/{firstName}")]
    public async Task<IActionResult> SearchAsync(string firstName)
    {
        if (string.IsNullOrWhiteSpace(firstName))
            return BadRequest("El texto de búsqueda es requerido.");

        var list = await _context.Employees
            .AsNoTracking()
            .Where(e =>
                (e.FirstName ?? string.Empty).Contains(firstName) ||
                (e.LastName ?? string.Empty).Contains(firstName))
            .ToListAsync();

        return Ok(list);
    }
}