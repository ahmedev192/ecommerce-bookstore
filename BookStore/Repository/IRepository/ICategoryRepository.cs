using BookStore.Models;

namespace BookStore.Repository.IRepository
{
    public interface ICategoryRepository : IRepository.IRepository<Category>
    {
        void Update(Category obj);
        void Save();
    }
}
