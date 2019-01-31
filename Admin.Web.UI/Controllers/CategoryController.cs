using Admin.BLL.Helpers;
using Admin.BLL.Repository;
using Admin.Models.Entities;
using Admin.Models.ViewModels;
using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class CategoryController : BaseController
    {
        public ActionResult Index()
        {
            return View();
        }

        [HttpGet]
        public ActionResult Add()
        {
            ViewBag.CategoryList = GetCategorySelectList();

            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Add(Category model)
        {
            try
            {
                model.TaxRate /= 100;
                if (model.SupCategoryID == 0) model.SupCategoryID = null;
                ViewBag.CategoryList = GetCategorySelectList();
                if (!ModelState.IsValid)
                {

                    model.TaxRate *= 100;
                    model.SupCategoryID = model.SupCategoryID ?? 0;
                    return View(model);
                }
                if (model.SupCategoryID > 0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryID).TaxRate;
                }
                new CategoryRepo().Insert(model);
                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla eklenmiştir.";
                return RedirectToAction("Add");
            }
            catch(DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata olustu : {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500

                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata olustu : {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500

                };
                return RedirectToAction("Error", "Home");
            }
            
        }

        [HttpGet]
        public ActionResult Update(int id=0)
        {
            ViewBag.CategoryList = GetCategorySelectList();
            var data = new CategoryRepo().GetById(id);
            if (data == null)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Kategori Bulunamadı",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 404
                };
                return RedirectToAction("Error", "Home");
            }
            return View(data);
        }

        Category data;

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult Update(Category model)
        {
            try
            {
                if (model.SupCategoryID == 0) model.SupCategoryID = null;
                if (!ModelState.IsValid)
                {
                    model.SupCategoryID = model.SupCategoryID ?? 0;
                    ViewBag.CategoryList = GetCategorySelectList();
                    return View(model);
                }
                if (model.SupCategoryID > 0)
                {
                    model.TaxRate = new CategoryRepo().GetById(model.SupCategoryID).TaxRate;
                }
                data = new CategoryRepo().GetById(model.Id);
                data.CategoryName = model.CategoryName;
                data.SupCategoryID = model.SupCategoryID;
                data.TaxRate = model.TaxRate;
                new CategoryRepo().Update(data);
                foreach (var dataCat in data.Categories)
                {
                    dataCat.TaxRate = data.TaxRate;
                    new CategoryRepo().Update(dataCat);
                    if (dataCat.Categories.Any())
                    {
                        UpdateSubTaxRate(dataCat.Categories);
                    }
                }
                

                TempData["Message"] = $"{model.CategoryName} isimli kategori başarıyla güncellenmiştir.";

                ViewBag.CategoryList = GetCategorySelectList();
                return View(data);
            }
            catch (DbEntityValidationException ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata olustu : {EntityHelpers.ValidationMessage(ex)}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500

                };
                return RedirectToAction("Error", "Home");
            }
            catch (Exception ex)
            {
                TempData["Model"] = new ErrorViewModel()
                {
                    Text = $"Bir hata olustu : {ex.Message}",
                    ActionName = "Add",
                    ControllerName = "Category",
                    ErrorCode = 500

                };
                return RedirectToAction("Error", "Home");
            }
            
        }
        void UpdateSubTaxRate(ICollection<Category> categories)
        {
            foreach (var dataCat in categories)
            {
                dataCat.TaxRate = data.TaxRate;
                new CategoryRepo().Update(dataCat);
                if (dataCat.Categories.Any())
                {
                    UpdateSubTaxRate(dataCat.Categories);
                }
            }
        }

    }
}