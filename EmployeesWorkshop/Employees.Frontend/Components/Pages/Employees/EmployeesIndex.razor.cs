using Employees.Frontend.Repositories;
using Microsoft.AspNetCore.Components;
using Employees.Shared.Entities;

namespace Employees.Frontend.Components.Pages.Employees;

public partial class EmployeesIndex
{
    [Inject] private IRepository Repository { get; set; } = null!;
    private List<Employee>? employees;

    protected override async Task OnInitializedAsync()
    {
        var httpsResult = await Repository.GetAsync<List<Employee>>("/api/employees");
        employees = httpsResult.Response;
    }
}