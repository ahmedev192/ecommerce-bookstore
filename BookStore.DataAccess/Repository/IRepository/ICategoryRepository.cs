using BookStore.Models;

namespace BookStore.DataAccess.Repository.IRepository
{
    public interface ICategoryRepository : IRepository.IRepository<Category>
    {
        void Update(Category obj);
    }
}
