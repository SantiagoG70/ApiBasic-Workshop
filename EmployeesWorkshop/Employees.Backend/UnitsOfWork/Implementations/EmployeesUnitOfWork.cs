using Employees.Backend.Repositories.Interfaces;
using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.DTOs;
using Employees.Shared.Entities;
using Employees.Shared.Responses;

namespace Employees.Backend.UnitsOfWork.Implementations;

public class EmployeesUnitOfWork : GenericUnitOfWork<Employee>, IEmployeesUnitOfWork
{
    private readonly IEmployeesRepository _employeesRepository;

    public EmployeesUnitOfWork(IGenericRepository<Employee> repository, IEmployeesRepository employeesRepository) : base(repository)
    {
        _employeesRepository = employeesRepository;
    }

    public override async Task<ActionResponse<int>> GetTotalRecordsAsync(PaginationDTO pagination) => await _employeesRepository.GetTotalRecordsAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync(PaginationDTO pagination) => await _employeesRepository.GetAsync(pagination);

    public override async Task<ActionResponse<IEnumerable<Employee>>> GetAsync() => await _employeesRepository.GetAsync();

    public virtual async Task<ActionResponse<Employee>> GetAsync(string firstName) => await _employeesRepository.GetAsync(firstName);
}