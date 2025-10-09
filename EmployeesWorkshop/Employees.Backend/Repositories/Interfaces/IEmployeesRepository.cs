using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.Repositories.Interfaces;

public interface IEmployeesRepository
{
    Task<ActionResponse<Employee>> GetAsync(string FirstName);

    Task<ActionResponse<IEnumerable<Employee>>> GetAsync();
}