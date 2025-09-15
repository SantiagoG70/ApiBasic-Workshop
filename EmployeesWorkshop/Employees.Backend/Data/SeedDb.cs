using DocuSign.eSign.Model;
using Employees.Shared.Entities;

namespace Employees.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;

    public SeedDb(DataContext context)
    {
        _context = context;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckEmployeesAsync();
    }

    private async Task CheckEmployeesAsync()
    {
        if (!_context.Employees.Any())
        {
            _context.Employees.Add(new Employee
            {
                FirstName = "Carlos",
                LastName = "Ramírez",
                IsActive = true,
                HireDate = new DateTime(2020, 3, 15),
                Salary = 2500000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "María",
                LastName = "López",
                IsActive = true,
                HireDate = new DateTime(2021, 6, 1),
                Salary = 3200000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Andrés",
                LastName = "González",
                IsActive = false,
                HireDate = new DateTime(2019, 11, 20),
                Salary = 1800000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Laura",
                LastName = "Martínez",
                IsActive = true,
                HireDate = new DateTime(2022, 1, 5),
                Salary = 2900000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Felipe",
                LastName = "Castro",
                IsActive = true,
                HireDate = new DateTime(2020, 9, 12),
                Salary = 3400000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Camila",
                LastName = "Hernández",
                IsActive = false,
                HireDate = new DateTime(2018, 7, 30),
                Salary = 2100000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Julián",
                LastName = "Mejía",
                IsActive = true,
                HireDate = new DateTime(2023, 4, 18),
                Salary = 2700000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Sofía",
                LastName = "Rojas",
                IsActive = true,
                HireDate = new DateTime(2021, 2, 14),
                Salary = 3100000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "David",
                LastName = "Torres",
                IsActive = true,
                HireDate = new DateTime(2020, 12, 10),
                Salary = 2600000m
            });

            _context.Employees.Add(new Employee
            {
                FirstName = "Paula",
                LastName = "Vargas",
                IsActive = true,
                HireDate = new DateTime(2022, 8, 22),
                Salary = 3000000m
            });
        }

        await _context.SaveChangesAsync();
    }
}