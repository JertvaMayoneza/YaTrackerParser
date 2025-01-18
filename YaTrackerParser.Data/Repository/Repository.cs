using System.Linq.Expressions;
using Microsoft.EntityFrameworkCore;
using YaTrackerParser.Contracts.Interfaces;
using YaTrackerParser.Data.Context;
using YaTrackerParser.Data.Context.Entites;

namespace YaTrackerParser.Data.Repository;

public class Repository<T> : IRepository<T> where T : class
{
    private readonly AppDbContext _context;
    private readonly DbSet<T> _dbSet;

    public Repository(AppDbContext context)
    {
        _context = context;
        _dbSet = context.Set<T>();
    }

    public async Task<IEnumerable<T>> GetAllAsync()
    {
        return await _dbSet.ToListAsync();
    }

    public async Task<T?> GetByIdAsync(int id)
    {
        return await _dbSet.FindAsync(id);
    }

    public async Task<IEnumerable<T>> FindAsync(Expression<Func<T, bool>> predicate)
    {
        return await _dbSet.Where(predicate).ToListAsync();
    }

    public async Task AddAsync(T entity)
    {
        await _dbSet.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(T entity)
    {
        _dbSet.Update(entity);
    }

    public async Task DeleteAsync(T entity)
    {
        _dbSet.Remove(entity);
    }

    public async Task SaveChangesAsync()
    {
        await _context.SaveChangesAsync();
    }

    public async Task<T?> GetOrCreateAsync(Expression<Func<T, bool>> predicate)
    {
        var existingEntity = await _dbSet.Where(predicate).FirstOrDefaultAsync();
        if (existingEntity != null)
        {
            return existingEntity;
        }

        var newEntity = Activator.CreateInstance<T>();
        if (newEntity is TicketEntity ticketEntity)
        {
            ticketEntity.TicketNumber = "TicketNumber";
            ticketEntity.Time = "Time";
            ticketEntity.Theme = "Theme";
            ticketEntity.Description = "Description";
            ticketEntity.UpdatedBy = "UpdatedBy";
        }
        await _dbSet.AddAsync(newEntity);
        await _context.SaveChangesAsync();

        return newEntity;
    }
}
