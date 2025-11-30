using Employees.Backend.Data;
using Employees.Backend.Helpers;
using Employees.Backend.Repositories.Interfaces;
using Employees.Backend.UnitsOfWork.Implementations;
using Employees.Shared.DTOs;
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

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination)
    {
        var queryable = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            var f = pagination.Filter.ToLower();
            queryable = queryable.Where(e =>
                e.FirstName.ToLower().Contains(f) ||
                e.LastName.ToLower().Contains(f));
        }

        var count = await queryable.CountAsync();

        return new ActionResponse<int>
        {
            WasSuccess = true,
            Result = count
        };
    }

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination)
    {
        var query = _context.Employees.AsQueryable();

        if (!string.IsNullOrWhiteSpace(pagination.Filter))
        {
            var f = pagination.Filter.ToLower();
            query = query.Where(x =>
                x.FirstName.ToLower().Contains(f) ||
                x.LastName.ToLower().Contains(f));
        }

        var employees = await query
            .OrderBy(x => x.FirstName).ThenBy(x => x.LastName)
            .Paginate(pagination)
            .ToListAsync();

        return new ActionResponse<IEnumerable<Employee>>
        {
            WasSuccess = true,
            Result = employees
        };
    }

    public virtual async Task<ActionResponse<Employee>> GetAsync(string firstName)
    {
        var employee = await _context.Employees
            .FirstOrDefaultAsync(s => s.FirstName == firstName);

        if (employee is null)
        {
            return new ActionResponse<Employee>
            {
                WasSuccess = false,
                Message = "Empleado no existe"
            };
        }

        return new ActionResponse<Employee>
        {
            WasSuccess = true,
            Result = employee
        };
    }
}