using Employees.Shared.Interfaces;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Employees.Shared.Entities;

public class Employee : IEntityWithName
{
    public int Id { get; set; }

    [Display(Name = "Nombre")]
    [Required(ErrorMessage = "El campo {0} debe de ser obligatorio")]
    [MaxLength(30, ErrorMessage = "el campo{0} no puede tener {1} caracteres")]
    public string FirstName { get; set; } = null!;

    [Display(Name = "Apellido")]
    [Required(ErrorMessage = "El campo {0} debe de ser obligatorio")]
    [MaxLength(30, ErrorMessage = "el campo{0} no puede tener {1} caracteres")]
    public string LastName { get; set; } = null!;

    [Display(Name = "Esta activo")]
    public bool IsActive { get; set; }

    [Display(Name = "Fecha de contratacion")]
    public DateTime HireDate { get; set; }

    [Display(Name = "Salario")]
    [Required(ErrorMessage = "El salario es obligatorio")]
    [Range(1000000, double.MaxValue, ErrorMessage = "el {0} no puede ser menor a {1} ")]
    [Column(TypeName = "decimal(18,2)")]
    public decimal Salary { get; set; }
}