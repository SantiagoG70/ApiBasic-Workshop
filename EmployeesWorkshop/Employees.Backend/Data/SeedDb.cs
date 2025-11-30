using Employees.Backend.UnitsOfWork.Interfaces;
using Employees.Shared.Entities;
using Employees.Shared.Enums;
using Microsoft.EntityFrameworkCore;

namespace Employees.Backend.Data;

public class SeedDb
{
    private readonly DataContext _context;
    private readonly IUsersUnitOfWork _usersUnitOfWork;

    public SeedDb(DataContext context, IUsersUnitOfWork usersUnitOfWork)
    {
        _context = context;
        _usersUnitOfWork = usersUnitOfWork;
    }

    public async Task SeedAsync()
    {
        await _context.Database.EnsureCreatedAsync();
        await CheckCountriesFullAsync();
        //await CheckCountriesAsync();
        await CheckRolesAsync();
        await CheckEmployeesAsync();
        await CheckUserAsync("1010", "Juan", "Zuluaga", "zulu@yopmail.com", "322 311 4620", "Calle Luna Calle Sol", UserType.Admin);
        await CheckUserAsync("123987", "Melany", "Alvarez", "melos@yopmail.com", "3002337562", "San Javier", UserType.Employee);
    }

    private async Task<User> CheckUserAsync(string document, string firstName, string lastName, string email, string phone, string address, UserType userType)
    {
        var user = await _usersUnitOfWork.GetUserAsync(email);
        if (user == null)
        {
            user = new User
            {
                FirstName = firstName,
                LastName = lastName,
                Email = email,
                UserName = email,
                PhoneNumber = phone,
                Address = address,
                Document = document,
                City = _context.Cities.FirstOrDefault(),
                UserType = userType,
            };

            await _usersUnitOfWork.AddUserAsync(user, "123456");
            await _usersUnitOfWork.AddUserToRoleAsync(user, userType.ToString());
        }

        return user;
    }

    private async Task CheckRolesAsync()
    {
        await _usersUnitOfWork.CheckRoleAsync(UserType.Admin.ToString());
        await _usersUnitOfWork.CheckRoleAsync(UserType.Employee.ToString());
    }

    private async Task CheckCountriesFullAsync()
    {
        if (!_context.Countries.Any())
        {
            var countriesSQLScript = File.ReadAllText("Data\\CountriesStatesCities.sql");
            await _context.Database.ExecuteSqlRawAsync(countriesSQLScript);
        }
    }

    private async Task CheckCountriesAsync()
    {
        if (!_context.Countries.Any())
        {
            _context.Countries.Add(new Country
            {
                FirstName = "Colombia",
                States = [
                    new State()
                    {
                        FirstName = "Antioquia",
                        Cities = [
                            new City() { FirstName = "Medellín" },
                            new City() { FirstName = "Itagüí" },
                            new City() { FirstName = "Envigado" },
                            new City() { FirstName = "Bello" },
                            new City() { FirstName = "Rionegro" },
                        ]
                    },
                    new State()
                    {
                        FirstName = "Bogotá",
                        Cities = [
                            new City() { FirstName = "Usaquen" },
                            new City() { FirstName = "Champinero" },
                            new City() { FirstName = "Santa fe" },
                            new City() { FirstName = "Useme" },
                            new City() { FirstName = "Bosa" },
                        ]
                    },
                ]
            });
            _context.Countries.Add(new Country
            {
                FirstName = "Estados Unidos",
                States = [
                    new State()
                {
                    FirstName = "Florida",
                    Cities = [
                        new City() { FirstName = "Orlando" },
                        new City() { FirstName = "Miami" },
                        new City() { FirstName = "Tampa" },
                        new City() { FirstName = "Fort Lauderdale" },
                        new City() { FirstName = "Key West" },
                    ]
                },
                new State()
                    {
                        FirstName = "Texas",
                        Cities = [
                            new City() { FirstName = "Houston" },
                            new City() { FirstName = "San Antonio" },
                            new City() { FirstName = "Dallas" },
                            new City() { FirstName = "Austin" },
                            new City() { FirstName = "El Paso" },
                        ]
                    },
                ]
            });
        }
        await _context.SaveChangesAsync();
    }

    private async Task CheckEmployeesAsync()
    {
        if (!_context.Employees.Any())
        {
            var employees = new List<Employee>
        {
            new Employee { FirstName = "Carlos", LastName = "Ramírez", IsActive = true, HireDate = new DateTime(2020, 3, 15), Salary = 2500000m },
            new Employee { FirstName = "María", LastName = "López", IsActive = true, HireDate = new DateTime(2021, 6, 1), Salary = 3200000m },
            new Employee { FirstName = "Andrés", LastName = "González", IsActive = false, HireDate = new DateTime(2019, 11, 20), Salary = 1800000m },
            new Employee { FirstName = "Laura", LastName = "Martínez", IsActive = true, HireDate = new DateTime(2022, 1, 5), Salary = 2900000m },
            new Employee { FirstName = "Felipe", LastName = "Castro", IsActive = true, HireDate = new DateTime(2020, 9, 12), Salary = 3400000m },
            new Employee { FirstName = "Camila", LastName = "Hernández", IsActive = false, HireDate = new DateTime(2018, 7, 30), Salary = 2100000m },
            new Employee { FirstName = "Julián", LastName = "Mejía", IsActive = true, HireDate = new DateTime(2023, 4, 18), Salary = 2700000m },
            new Employee { FirstName = "Sofía", LastName = "Rojas", IsActive = true, HireDate = new DateTime(2021, 2, 14), Salary = 3100000m },
            new Employee { FirstName = "David", LastName = "Torres", IsActive = true, HireDate = new DateTime(2020, 12, 10), Salary = 2600000m },
            new Employee { FirstName = "Paula", LastName = "Vargas", IsActive = true, HireDate = new DateTime(2022, 8, 22), Salary = 3000000m },

            new Employee { FirstName = "Ricardo", LastName = "Gómez", IsActive = true, HireDate = new DateTime(2019, 5, 2), Salary = 2800000m },
            new Employee { FirstName = "Natalia", LastName = "Pérez", IsActive = true, HireDate = new DateTime(2023, 1, 11), Salary = 3300000m },
            new Employee { FirstName = "Sebastián", LastName = "Díaz", IsActive = true, HireDate = new DateTime(2021, 3, 19), Salary = 2950000m },
            new Employee { FirstName = "Valentina", LastName = "Cárdenas", IsActive = false, HireDate = new DateTime(2018, 12, 7), Salary = 2000000m },
            new Employee { FirstName = "Daniel", LastName = "Ortiz", IsActive = true, HireDate = new DateTime(2022, 5, 25), Salary = 3150000m },
            new Employee { FirstName = "Mónica", LastName = "Suárez", IsActive = true, HireDate = new DateTime(2020, 2, 17), Salary = 2800000m },
            new Employee { FirstName = "Esteban", LastName = "Cruz", IsActive = false, HireDate = new DateTime(2017, 10, 10), Salary = 1900000m },
            new Employee { FirstName = "Lucía", LastName = "Morales", IsActive = true, HireDate = new DateTime(2023, 6, 4), Salary = 3050000m },
            new Employee { FirstName = "Mateo", LastName = "Vélez", IsActive = true, HireDate = new DateTime(2021, 9, 13), Salary = 2850000m },
            new Employee { FirstName = "Isabella", LastName = "Gil", IsActive = true, HireDate = new DateTime(2020, 7, 30), Salary = 3500000m },

            new Employee { FirstName = "Santiago", LastName = "Muñoz", IsActive = true, HireDate = new DateTime(2019, 8, 16), Salary = 3000000m },
            new Employee { FirstName = "Angela", LastName = "Patiño", IsActive = false, HireDate = new DateTime(2018, 4, 12), Salary = 2300000m },
            new Employee { FirstName = "Tomás", LastName = "Acosta", IsActive = true, HireDate = new DateTime(2022, 11, 1), Salary = 3250000m },
            new Employee { FirstName = "Sara", LastName = "Quintero", IsActive = true, HireDate = new DateTime(2023, 3, 7), Salary = 3100000m },
            new Employee { FirstName = "Jorge", LastName = "Lara", IsActive = true, HireDate = new DateTime(2019, 1, 27), Salary = 2900000m },
            new Employee { FirstName = "Diana", LastName = "León", IsActive = false, HireDate = new DateTime(2017, 9, 22), Salary = 2200000m },
            new Employee { FirstName = "Oscar", LastName = "Jiménez", IsActive = true, HireDate = new DateTime(2020, 5, 8), Salary = 3100000m },
            new Employee { FirstName = "Tatiana", LastName = "Rincón", IsActive = true, HireDate = new DateTime(2021, 10, 6), Salary = 2700000m },
            new Employee { FirstName = "Héctor", LastName = "Navarro", IsActive = false, HireDate = new DateTime(2018, 2, 18), Salary = 2400000m },
            new Employee { FirstName = "Patricia", LastName = "Beltrán", IsActive = true, HireDate = new DateTime(2022, 9, 9), Salary = 3300000m },

            new Employee { FirstName = "Miguel", LastName = "Cardona", IsActive = true, HireDate = new DateTime(2023, 1, 25), Salary = 2950000m },
            new Employee { FirstName = "Carolina", LastName = "Herrera", IsActive = true, HireDate = new DateTime(2019, 7, 14), Salary = 3100000m },
            new Employee { FirstName = "Javier", LastName = "Ruiz", IsActive = false, HireDate = new DateTime(2017, 5, 20), Salary = 2000000m },
            new Employee { FirstName = "Claudia", LastName = "Mendoza", IsActive = true, HireDate = new DateTime(2021, 8, 28), Salary = 2900000m },
            new Employee { FirstName = "Pablo", LastName = "Zapata", IsActive = true, HireDate = new DateTime(2020, 6, 2), Salary = 3400000m },
            new Employee { FirstName = "Eliana", LastName = "Cano", IsActive = true, HireDate = new DateTime(2023, 5, 10), Salary = 2750000m },
            new Employee { FirstName = "Fernando", LastName = "Montoya", IsActive = true, HireDate = new DateTime(2022, 2, 19), Salary = 3200000m },
            new Employee { FirstName = "Gloria", LastName = "Salazar", IsActive = false, HireDate = new DateTime(2018, 1, 30), Salary = 2100000m },
            new Employee { FirstName = "Iván", LastName = "Espinosa", IsActive = true, HireDate = new DateTime(2020, 3, 11), Salary = 3000000m },
            new Employee { FirstName = "Daniela", LastName = "Reyes", IsActive = true, HireDate = new DateTime(2021, 11, 21), Salary = 2850000m },

            new Employee { FirstName = "Luis", LastName = "Téllez", IsActive = true, HireDate = new DateTime(2023, 6, 5), Salary = 3150000m },
            new Employee { FirstName = "Natalia", LastName = "Serrano", IsActive = false, HireDate = new DateTime(2018, 9, 16), Salary = 2000000m },
            new Employee { FirstName = "Rafael", LastName = "Arboleda", IsActive = true, HireDate = new DateTime(2022, 10, 3), Salary = 3300000m },
            new Employee { FirstName = "Andrea", LastName = "Restrepo", IsActive = true, HireDate = new DateTime(2020, 12, 8), Salary = 2950000m },
            new Employee { FirstName = "Camilo", LastName = "Agudelo", IsActive = true, HireDate = new DateTime(2021, 4, 22), Salary = 3100000m },
            new Employee { FirstName = "Melissa", LastName = "Gaitán", IsActive = false, HireDate = new DateTime(2019, 5, 25), Salary = 2300000m },
            new Employee { FirstName = "Jhon", LastName = "Parra", IsActive = true, HireDate = new DateTime(2023, 2, 14), Salary = 3400000m },
            new Employee { FirstName = "Catalina", LastName = "Giraldo", IsActive = true, HireDate = new DateTime(2021, 7, 17), Salary = 2800000m },
            new Employee { FirstName = "Nelson", LastName = "Ramírez", IsActive = true, HireDate = new DateTime(2022, 1, 29), Salary = 3000000m },
            new Employee { FirstName = "Yuliana", LastName = "Cortés", IsActive = true, HireDate = new DateTime(2020, 11, 9), Salary = 3150000m },
        };

            _context.Employees.AddRange(employees);
            await _context.SaveChangesAsync();
        }
    }
}