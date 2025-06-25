using Microsoft.EntityFrameworkCore;

namespace ecommerce_bookstore.Models.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext( DbContextOptions<ApplicationDbContext> options ) : base(options)
        {
            
        }

        public DbSet<Category> Categories { get; set; }
    }
}
