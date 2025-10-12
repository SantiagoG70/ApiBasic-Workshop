namespace Employees.Shared.DTOs;

public class PaginationDTO
{
    public string? Filter { get; set; }

    public int Id { get; set; }

    public int Page { get; set; } = 1;

    public int RecordsNumber { get; set; } = 10;
}