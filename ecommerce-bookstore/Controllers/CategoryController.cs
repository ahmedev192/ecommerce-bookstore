using ecommerce_bookstore.Models;
using ecommerce_bookstore.Models.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.EntityFrameworkCore;

namespace ecommerce_bookstore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ApplicationDbContext _db;
        public CategoryController(ApplicationDbContext db)
        {
            _db = db;
        }
        public async Task<IActionResult> Index()
        {
            List<Category> objCategoryList = await _db.Categories.ToListAsync();
            return View(objCategoryList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom ", "Name can't be equal Display Order");
            }

            if (ModelState.IsValid)
            {
                await _db.Categories.AddAsync(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category created successfully";

                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }


        [HttpGet]
        public IActionResult Edit(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost]
        public async Task<IActionResult> Edit(Category obj)
        {


            if (ModelState.IsValid)
            {
                _db.Categories.Update(obj);
                await _db.SaveChangesAsync();
                TempData["success"] = "Category updated successfully";


                return RedirectToAction("Index");
            }
            else
            {
                return View(obj);
            }
        }



        [HttpGet]
        public IActionResult Delete(int id)
        {
            if (id <= 0)
            {
                return NotFound();
            }
            Category? categoryFromDB = _db.Categories.FirstOrDefault(c => c.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public async Task<IActionResult> DeletePost(int id)
        {


            Category? obj = _db.Categories.Find(id);


            _db.Categories.Remove(obj);
            await _db.SaveChangesAsync();
            TempData["success"] = "Category deleted successfully";


            return RedirectToAction("Index");


        }














    }
}
