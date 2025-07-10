using BookStore.DataAccess.Repository.IRepository;
using BookStore.Models;
using BookStore.Utilities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace BookStore.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = SD.Role_Admin)]
    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;

        public CompanyController(IUnitOfWork db)
        {
            _unitOfWork = db;
        }
        public IActionResult Index()
        {
            List<Company> objCompanyList = _unitOfWork.Company.GetAll().ToList();
            return View(objCompanyList);
        }

        [HttpGet]
        public IActionResult Upsert(int? id)
        {
            if (!id.HasValue)
            {
                return View();
            }
            else
            {
                Company? CompanyFromDB = _unitOfWork.Company.Get(c => c.Id == id);
                if (CompanyFromDB == null)
                {
                    return NotFound();
                }
                return View(CompanyFromDB);
            }
        }


        [HttpPost]
        [ValidateAntiForgeryToken]

        public IActionResult Upsert(Company obj)
        {

            if (ModelState.IsValid)
            {
                if (obj.Id == 0)
                {


                    _unitOfWork.Company.Add(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company created successfully";

                    return RedirectToAction("Index");


                }
                else
                {

                    _unitOfWork.Company.Update(obj);
                    _unitOfWork.Save();
                    TempData["success"] = "Company updated successfully";


                    return RedirectToAction("Index");

                }

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
            Company? CompanyFromDB = _unitOfWork.Company.Get(c => c.Id == id);
            if (CompanyFromDB == null)
            {
                return NotFound();
            }
            return View(CompanyFromDB);
        }
        [HttpPost, ActionName("Delete")]
        public IActionResult DeletePost(int id)
        {


            Company? obj = _unitOfWork.Company.Get(e => e.Id == id);


            _unitOfWork.Company.Remove(obj);
            _unitOfWork.Save();
            TempData["success"] = "Company deleted successfully";


            return RedirectToAction("Index");


        }



    }
}
