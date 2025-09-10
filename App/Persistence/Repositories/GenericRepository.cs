using Microsoft.EntityFrameworkCore;
using Domain.Common;
using Persistence.Interfaces;

namespace Persistence.Repositories;

public class GenericRepository<TEntity, TKey>(AppDbContext context) : IGenericRepository<TEntity, TKey> where TEntity : BaseEntity<TKey>
{
    protected readonly AppDbContext _context = context;

    public async Task<IReadOnlyList<TEntity>> GetAsync()
    {
        return await _context.Set<TEntity>().AsNoTracking().ToListAsync();
    }
    public async Task<TEntity> GetByIdAsync(TKey id)
    {
        return await _context.Set<TEntity>().AsNoTracking().FirstOrDefaultAsync(q => q.Id!.Equals(id));
    }

    public async Task CreateAsync(TEntity entity)
    {
        await _context.AddAsync(entity);
        await _context.SaveChangesAsync();
    }

    public async Task DeleteAsync(TEntity entity)
    {
        _context.Remove(entity);
        await _context.SaveChangesAsync();
    }

    public async Task UpdateAsync(TEntity entity)
    {
        _context.Entry(entity).State = EntityState.Modified;
        await _context.SaveChangesAsync();
    }
}