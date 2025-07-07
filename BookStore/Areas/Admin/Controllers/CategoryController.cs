using BookStore.DataAccess.Data;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class CategoryController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CategoryController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public  IActionResult Index()
        {
            List<Category> objCategoryList = _unitOfWork.Category.GetAll().ToList();
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
                _unitOfWork.Category.Add(obj);
                 _unitOfWork.Save();
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
            Category? categoryFromDB = _unitOfWork.Category.Get(c => c.Id == id);
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
                _unitOfWork.Category.Update(obj);
                 _unitOfWork.Save();
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
            Category? categoryFromDB = _unitOfWork.Category.Get(c => c.Id == id);
            if (categoryFromDB == null)
            {
                return NotFound();
            }
            return View(categoryFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public  IActionResult DeletePost(int id)
        {


            Category? obj = _unitOfWork.Category.Get(e=> e.Id == id);


            _unitOfWork.Category.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Category deleted successfully";


            return RedirectToAction("Index");


        }














    }
}
