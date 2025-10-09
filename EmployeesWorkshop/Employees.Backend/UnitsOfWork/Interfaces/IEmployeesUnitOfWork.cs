using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.UnitsOfWork.Interfaces;

public interface IEmployeesUnitOfWork
{
    Task<ActionResponse<Employee>> GetAsync(string FirstName);

    Task<ActionResponse<IEnumerable<Employee>>> GetAsync();
}