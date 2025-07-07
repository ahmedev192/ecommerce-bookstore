using System.Linq.Expressions;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface IRepository<T> where  T : class
    {
        // Define methods for the repository interface
        IEnumerable<T> GetAll();
        T Get(Expression<Func<T , bool>> filter );
        void Add(T entity);
        void Remove(T entity);
        void RemoveRange(IEnumerable<T> entities);

    }
}
