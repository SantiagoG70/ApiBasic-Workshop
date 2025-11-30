using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.DTOs;
using Employees.Shared.Entities;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Employees.Backend.Controllers
{
    [ApiController]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    [Route("api/[controller]")]
    public class EmployeesController : GenericController<Employee>
    {
        private readonly IEmployeesUnitOfWork _employeesUnitOfWork;

        public EmployeesController(IGenericUnitOfWork<Employee> unit, IEmployeesUnitOfWork employeesUnitOfWork) : base(unit)
        {
            _employeesUnitOfWork = employeesUnitOfWork;
        }

        [AllowAnonymous]
        [HttpGet("paginated")]
        public override async Task<IActionResult> GetAsync(PaginationDTO pagination)
        {
            var response = await _employeesUnitOfWork.GetAsync(pagination);
            if (response.WasSuccess)
            {
                return Ok(response.Result);
            }
            return BadRequest();
        }

        [HttpGet]
        public override async Task<IActionResult> GetAsync()
        {
            var response = await _employeesUnitOfWork.GetAsync();
            if (response.WasSuccess)
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

            if (!response.WasSuccess || response.Result == null)
                return BadRequest(response.Message);

            var filtered = response.Result
                .Where(e => e.FirstName.Contains(firstName, StringComparison.OrdinalIgnoreCase))
                .ToList();

            return Ok(filtered);
        }

        [HttpGet("totalRecords")]
        public override async Task<IActionResult> GetTotalRecordsAsync([FromQuery] PaginationDTO pagination)
        {
            var action = await _employeesUnitOfWork.GetTotalRecordsAsync(pagination);
            if (action.WasSuccess)
            {
                return Ok(action.Result);
            }
            return BadRequest();
        }
    }
}