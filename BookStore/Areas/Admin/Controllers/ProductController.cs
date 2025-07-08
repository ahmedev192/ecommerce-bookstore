using Microsoft.AspNetCore.Mvc.Rendering;
using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Models.ViewModels;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class ProductController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IWebHostEnvironment _webHostEnvironment;
        public ProductController(IUnitOfWork unitOfWork, IWebHostEnvironment webHostEnvironment)
        {
            _unitOfWork = unitOfWork;
            _webHostEnvironment = webHostEnvironment;

        }
        public IActionResult Index()
        {
            List<Product> objProductList = _unitOfWork.Product.GetAll().ToList();
            return View(objProductList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            ProductVM productVM = new()
            {
                CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                }),
                Product = new Product()
            };
            if (!id.HasValue)
            {
                //create
                return View(productVM);
            }
            else
            {
                //update
                productVM.Product = _unitOfWork.Product.Get(u => u.Id == id);
                return View(productVM);
            }

        }


        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Upsert(ProductVM productVM, IFormFile? file)
        {
            if (ModelState.IsValid)
            {
                string wwwRootPath = _webHostEnvironment.WebRootPath;
                string productFolderPath = Path.Combine(wwwRootPath, "images", "products");
                string fileName = file != null ? Guid.NewGuid().ToString() + Path.GetExtension(file.FileName) : string.Empty;

                if (productVM.Product.Id == 0)
                {
                    // CREATE
                    if (file != null)
                    {
                        using (var fileStream = new FileStream(Path.Combine(productFolderPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }
                        productVM.Product.ImageUrl = @"\images\products\" + fileName;
                    }

                    _unitOfWork.Product.Add(productVM.Product);
                    TempData["success"] = "Product created successfully";
                }
                else
                {
                    // UPDATE
                    var productFromDb = _unitOfWork.Product.Get(u => u.Id == productVM.Product.Id);

                    if (productFromDb == null)
                    {
                        return NotFound();
                    }

                    if (file != null)
                    {
                        // Delete old image
                        if (!string.IsNullOrEmpty(productFromDb.ImageUrl))
                        {
                            var oldImagePath = Path.Combine(wwwRootPath, productFromDb.ImageUrl.TrimStart('\\'));
                            if (System.IO.File.Exists(oldImagePath))
                            {
                                System.IO.File.Delete(oldImagePath);
                            }
                        }

                        // Save new image
                        using (var fileStream = new FileStream(Path.Combine(productFolderPath, fileName), FileMode.Create))
                        {
                            file.CopyTo(fileStream);
                        }

                        productFromDb.ImageUrl = @"\images\products\" + fileName;
                    }

                    // Update other properties
                    productFromDb.Title = productVM.Product.Title;
                    productFromDb.Description = productVM.Product.Description;
                    productFromDb.ISBN = productVM.Product.ISBN;
                    productFromDb.Author = productVM.Product.Author;
                    productFromDb.ListPrice = productVM.Product.ListPrice;
                    productFromDb.Price = productVM.Product.Price;
                    productFromDb.Price50 = productVM.Product.Price50;
                    productFromDb.Price100 = productVM.Product.Price100;
                    productFromDb.CategoryId = productVM.Product.CategoryId;

                    _unitOfWork.Product.Update(productFromDb);
                    TempData["success"] = "Product updated successfully";
                }

                _unitOfWork.Save();
                return RedirectToAction("Index");
            }
            else
            {
                productVM.CategoryList = _unitOfWork.Category.GetAll().Select(u => new SelectListItem
                {
                    Text = u.Name,
                    Value = u.Id.ToString()
                });

                return View(productVM);
            }
        }


        public IActionResult Delete(int? id)
        {
            if (id == null || id == 0)
            {
                return NotFound();
            }
            Product? productFromDb = _unitOfWork.Product.Get(u => u.Id == id);

            if (productFromDb == null)
            {
                return NotFound();
            }
            return View(productFromDb);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePOST(int? id)
        {
            Product? obj = _unitOfWork.Product.Get(u => u.Id == id);
            if (obj == null)
            {
                return NotFound();
            }
            _unitOfWork.Product.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Product deleted successfully";
            return RedirectToAction("Index");
        }
    }
}
