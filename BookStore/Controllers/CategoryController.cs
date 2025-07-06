using BookStore.DataAccess.Data;
using BookStore.Models;
using BookStore.Repository.IRepository;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Controllers
{
    public class CategoryController : Controller
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository db)
        {
            _categoryRepo = db;
        }
        public  IActionResult Index()
        {
            List<Category> objCategoryList =  _categoryRepo.GetAll().ToList();
            return View(objCategoryList);
        }
        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }
        [HttpPost]
        public  IActionResult Create(Category obj)
        {
            if (obj.Name == obj.DisplayOrder.ToString())
            {
                ModelState.AddModelError("Custom ", "Name can't be equal Display Order");
            }

            if (ModelState.IsValid)
            {
                 _categoryRepo.Add(obj);
                 _categoryRepo.Save();
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
            Category? categoryFromDB = _categoryRepo.Get(c => c.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost]
        public  IActionResult Edit(Category obj)
        {


            if (ModelState.IsValid)
            {
                _categoryRepo.Update(obj);
                 _categoryRepo.Save();
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
            Category? categoryFromDB = _categoryRepo.Get(c => c.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public  IActionResult DeletePost(int id)
        {


            Category? obj = _categoryRepo.Get(e=> e.Id == id);


            _categoryRepo.Remove(obj);
            _categoryRepo.Save();
            TempData["success"] = "Category deleted successfully";


            return RedirectToAction("Index");


        }














    }
}
