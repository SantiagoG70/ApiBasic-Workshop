using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Backend.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmployeesController : GenericController<Employee>
    {
        private readonly IEmployeesUnitOfWork _employeesUnitOfWork;

        public EmployeesController(IGenericUnitOfWork<Employee> unit, IEmployeesUnitOfWork employeesUnitOfWork) : base(unit)
        {
            _employeesUnitOfWork = employeesUnitOfWork;
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _employeesUnitOfWork.GetAsync();
            if (response.WasSucces)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet("search/{firstName}")]
        public async Task<IActionResult> SearchAsync(string firstName)
        {
            if (string.IsNullOrWhiteSpace(firstName))
                return BadRequest("El texto de búsqueda es requerido.");

            var response = await _employeesUnitOfWork.GetAsync();

            if (!response.WasSucces || response.Result == null)
                return BadRequest(response.Message);

            var filtered = response.Result
                .Where(e => e.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(filtered);
        }
    }
}