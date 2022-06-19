using System.Linq.Expressions;

namespace Translator.Domain.Interfaces
{
    /// <summary>
    /// Generic Repository interface
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface IRepository<T> where T : IEntity
    {
        Task Add(T t);
        Task Save();
        Task<IReadOnlyCollection<T>> GetAll();
        Task<int> GetAllCount();
        Task<IReadOnlyCollection<T>> GetByFilter(Expression<Func<T, bool>> filter,
            Func<IQueryable<T>, IOrderedQueryable<T>>? orderBy = null,
            string includeProperties = "",
            int first = 0, int offset = 0);
        Task<int> GetCountByFilter(Expression<Func<T, bool>> filter);
    }
}
