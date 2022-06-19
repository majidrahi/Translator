using Translator.Data.Context;
using Translator.Domain.Interfaces;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Translator.Data.Repositories
{
    /// <summary>
    /// Generic Repository
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class Repository<T> : IRepository<T> where T : class, IEntity
    {
        private readonly TranslatorDbContext _context;
        private readonly DbSet<T> _dbSet;

        public Repository(TranslatorDbContext context)
        {
            this._context = context;
            this._dbSet = _context.Set<T>();
        }

        public async Task Add(T t)
        {
            await _dbSet.AddAsync(t);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetAll()
        {
            return await _dbSet.AsNoTracking().ToListAsync();
        }

        public async Task<IReadOnlyCollection<T>> GetByFilter(Expression<Func<T, bool>> filter, Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int first = 0, int offset = 0)
        {
            IQueryable<T> query = _dbSet;

            if (filter != null)
            {
                query = query.Where(filter);
            }

            if (offset > 0)
            {
                query = query.Skip(offset);
            }
            if (first > 0)
            {
                query = query.Take(first);
            }

            foreach (var includeProperty in includeProperties.Split
                (new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries))
            {
                query = query.Include(includeProperty);
            }

            if (orderBy != null)
            {
                return await orderBy(query).ToListAsync();
            }
            else
            {
                return await query.ToListAsync();
            }
        }

        public async Task<int> GetCountByFilter(Expression<Func<T, bool>> filter)
        {
            return await _dbSet.Where(filter).CountAsync();
        }

        public async Task<int> GetAllCount()
        {
            return await _dbSet.CountAsync();
        }
    }
}
