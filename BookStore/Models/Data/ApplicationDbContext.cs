using Microsoft.EntityFrameworkCore;

namespace BookStore.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}
