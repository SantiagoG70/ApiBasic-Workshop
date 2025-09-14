using System.ComponentModel.DataAnnotations;

namespace Employees.Shared.Entities;

public class Employee
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} debe de ser obligatorio")]
    [MaxLength(30, ErrorMessage = "Maximo 30 Caracteres")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El campo {0} debe de ser obligatorio")]
    [MaxLength(30, ErrorMessage = "Maximo 30 Caracteres")]
    public string? LastName { get; set; }

    [Display(Name = "Esta activo")]
    public bool IsActive { get; set; }

    [Display(Name = "Fecha de contratacion")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Salario")]
    [Required(ErrorMessage = "El salario es obligatorio")]
    [Range(1000000, double.MaxValue, ErrorMessage = "El salario debe ser mínimo de $1,000,000")]
    public decimal Salary { get; set; }
}