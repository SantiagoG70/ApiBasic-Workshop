using Employees.Shared.DTOs;
using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.Repositories.Interfaces;

public interface IEmployeesRepository
{
    Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination);

    Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination);

    Task<ActionResponse<Employee>> GetAsync(string FirstName);

    Task<ActionResponse<IEnumerable<Employee>>> GetAsync();
}