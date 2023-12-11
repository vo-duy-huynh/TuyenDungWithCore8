using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using TuyenDungWeb.DataAccess.Repositories.IRepository;
using TuyenDungWeb.Model;
using TuyenDungWeb.Model.ViewModels;

namespace TuyenDungWeb.MVC.Areas.Admin.Controllers
{

    public class CompanyController : Controller
    {
        private readonly IUnitOfWork _unitOfWork;
        public CompanyController(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public ActionResult Index()
        {
            var listCom = _unitOfWork.Company.GetAll();
            return View(listCom);
        }


        public IActionResult Upsert(int? id)
        {

            if (id == null || id == 0)
            {
                ViewBag.Companies = _unitOfWork.Company.GetAll();
                //create
                return View(new CompanyVM());
            }
            else
            {
                var model = new CompanyVM
                {
                    Company = _unitOfWork.Company.Get(u => u.Id == id),
                    Tags = _unitOfWork.Tag.GetAll().Select(x => new SelectListItem { Text = x.Name, Value = x.Id.ToString() }),
                };
                return View(model);
            }

        }
        [HttpPost]
        public IActionResult Upsert(Company CompanyObj)
        {
            if (ModelState.IsValid)
            {

                if (CompanyObj.Id == 0)
                {
                    _unitOfWork.Company.Add(CompanyObj);
                }
                else
                {
                    _unitOfWork.Company.Update(CompanyObj);
                }

                _unitOfWork.Save();
                TempData["success"] = "Company created successfully";
                return RedirectToAction("Index");
            }
            else
            {

                return View(CompanyObj);
            }
        }
        public ActionResult Delete(int id, IFormCollection collection)
        {
            try
            {
                return RedirectToAction(nameof(Index));
            }
            catch
            {
                return View();
            }
        }
    }
}
