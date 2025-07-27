using BookStore.Models;
using BookStore.Utilities;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
namespace BookStore.DataAccess.DbInitializer
{
    public class DbInitializer : IDbInitializer
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly ApplicationDbContext _db;

        public DbInitializer(
            UserManager<IdentityUser> userManager,
            RoleManager<IdentityRole> roleManager,
            ApplicationDbContext db)
        {
            _roleManager = roleManager;
            _userManager = userManager;
            _db = db;
        }

        public void Initialize()
        {
            try
            {
                if (_db.Database.GetPendingMigrations().Any())
                {
                    _db.Database.Migrate();
                }
            }
            catch (Exception)
            {
                // Log exception if needed
            }

            // Create roles and admin user
            if (!_roleManager.RoleExistsAsync(SD.Role_Admin).GetAwaiter().GetResult())
            {
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Admin)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_Employee)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Indi)).GetAwaiter().GetResult();
                _roleManager.CreateAsync(new IdentityRole(SD.Role_User_Comp)).GetAwaiter().GetResult();

                var adminUser = new ApplicationUser
                {
                    UserName = "admin@gmail.com",
                    Email = "admin@gmail.com",
                    Name = "Ahmed Mahmoud",
                    PhoneNumber = "01118672736",
                    StreetAddress = "test 123",
                    State = "Cairo",
                    PostalCode = "12",
                    City = "Cairo"
                };

                _userManager.CreateAsync(adminUser, "Admin123*").GetAwaiter().GetResult();
                var user = _db.ApplicationUsers.FirstOrDefault(u => u.Email == "admin@gmail.com");
                _userManager.AddToRoleAsync(user, SD.Role_Admin).GetAwaiter().GetResult();
            }

            // Seed Category
            if (!_db.Categories.Any())
            {
                _db.Categories.AddRange(
                    new Category { Name = "Action", DisplayOrder = 1 },
                    new Category { Name = "SciFi", DisplayOrder = 2 },
                    new Category { Name = "History", DisplayOrder = 3 }
                );
                _db.SaveChanges();
            }

            // Seed CoverType
            if (!_db.CoverTypes.Any())
            {
                _db.CoverTypes.AddRange(
                    new CoverType { Name = "Hardcover" },
                    new CoverType { Name = "Paperback" },
                    new CoverType { Name = "Digital" }
                );
                _db.SaveChanges();
            }

            // Seed Company
            if (!_db.Companies.Any())
            {
                _db.Companies.AddRange(
                    new Company { Name = "Tech Solution", City = "Cairo", StreetAddress = "123 Tech Ave", PostalCode = "12345", State = "CA", PhoneNumber = "1234567890" },
                    new Company { Name = "Book House", City = "Giza", StreetAddress = "456 Book Blvd", PostalCode = "67890", State = "NY", PhoneNumber = "0987654321" }
                );
                _db.SaveChanges();
            }

            // Seed Product
            if (!_db.Products.Any())
            {
                var category = _db.Categories.First();
                var coverType = _db.CoverTypes.First();

                _db.Products.AddRange(
                    new Product
                    {
                        Title = "Fortune of Time",
                        Author = "Billy Spark",
                        Description = "A thrilling adventure.",
                        ISBN = "SWD9999001",
                        ListPrice = 99,
                        Price = 90,
                        Price50 = 85,
                        Price100 = 80,
                        CategoryId = category.Id,
                        CoverTypeId = coverType.Id,
                        ImageUrl = ""
                    },
                    new Product
                    {
                        Title = "Dark Skies",
                        Author = "Nancy Hoover",
                        Description = "Mystery in the skies.",
                        ISBN = "CAW777777701",
                        ListPrice = 40,
                        Price = 30,
                        Price50 = 25,
                        Price100 = 20,
                        CategoryId = category.Id,
                        CoverTypeId = coverType.Id,
                        ImageUrl = ""

                    }
                );
                _db.SaveChanges();
            }
        }
    }
}
