using Employees.Backend.Data;
using Employees.Backend.Repositories.Interfaces;
using Employees.Backend.UnitsOfWork.Implementations;
using Employees.Shared.Entities;
using Employees.Shared.Responses;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Repositories.Implementations;

public class EmployeesRepository : GenericRepository<Employee>, IEmployeesRepository
{
    private readonly DataContext _context;

    public EmployeesRepository(DataContext context) : base(context)
    {
        _context = context;
    }

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync()
    {
        var employees = await _context.Employees
            .OrderBy(x => x.FirstName)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Employee>>
        {
            WasSucces = true,
            Result = employees
        };
    }

    public virtual async Task<ActionResponse<Employee>> GetAsync(string FirstName)
    {
        var Employee = await _context.Employees
             .Include(s => s.FirstName)
             .FirstOrDefaultAsync(s => s.FirstName == FirstName);

        if (Employee == null)
        {
            return new ActionResponse<Employee>
            {
                WasSucces = false,
                Message = "Estado no existe"
            };
        }

        return new ActionResponse<Employee>
        {
            WasSucces = true,
            Result = Employee
        };
    }
}