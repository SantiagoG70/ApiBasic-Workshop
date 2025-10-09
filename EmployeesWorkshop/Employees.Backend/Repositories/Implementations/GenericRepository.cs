using Employees.Backend.Data;
using Employees.Shared.Responses;
using Microsoft.EntityFrameworkCore;
using Employees.Backend.Repositories.Interfaces;

namespace Employees.Backend.UnitsOfWork.Implementations;

public class GenericRepository<T> : IGenericRepository<T> where T : class
{
    private readonly DataContext _context;
    private readonly DbSet<T> _entity;

    public GenericRepository(DataContext context)
    {
        _context = context;
        _entity = context.Set<T>();
    }

    public virtual async Task<ActionResponse<T>> AddAsync(T entity)
    {
        _context.Add(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSucces = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exeception)
        {
            return ExceptionActionResponse(exeception);
        }
    }

    public virtual async Task<ActionResponse<T>> DeleteAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Message = "Registro no encontrado"
            };
        }
        _entity.Remove(row);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSucces = true,
                Result = row
            };
        }
        catch
        {
            return new ActionResponse<T>
            {
                Message = "No se puede borrar porque tiene reistros relacionados"
            };
        }
    }

    public virtual async Task<ActionResponse<T>> GetAsync(int id)
    {
        var row = await _entity.FindAsync(id);
        if (row == null)
        {
            return new ActionResponse<T>
            {
                Message = "Registro no encontrado"
            };
        }
        return new ActionResponse<T>
        {
            WasSucces = true,
            Result = row
        };
    }

    public virtual async Task<ActionResponse<IEnumerable<T>>> GetAsync() =>
    new ActionResponse<IEnumerable<T>>
    {
        WasSucces = true,
        Result = await _entity.ToListAsync()
    };

    public async Task<ActionResponse<T>> UpdateAsync(T entity)
    {
        _context.Update(entity);
        try
        {
            await _context.SaveChangesAsync();
            return new ActionResponse<T>
            {
                WasSucces = true,
                Result = entity
            };
        }
        catch (DbUpdateException)
        {
            return DbUpdateExceptionActionResponse();
        }
        catch (Exception exeception)
        {
            return ExceptionActionResponse(exeception);
        }
    }

    private ActionResponse<T> ExceptionActionResponse(Exception exeception) =>
    new ActionResponse<T>
    {
        Message = exeception.Message,
    };

    private ActionResponse<T> DbUpdateExceptionActionResponse() =>
    new ActionResponse<T>
    {
        Message = "Ya existe el registro"
    };
}