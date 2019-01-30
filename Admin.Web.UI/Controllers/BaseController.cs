using Admin.BLL.Repository;
using Admin.Models.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Admin.Web.UI.Controllers
{
    public class BaseController : Controller
    {
        protected List<SelectListItem> GetCategorySelectList()
        {
            var categories = new CategoryRepo().GetAll().OrderBy(x => x.CategoryName);
            List<SelectListItem> list = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Üst kategorisi yok",
                    Value = "0"
                }
            };

            foreach (var cat in categories)
            {
                if (cat.Categories.Any())
                {
                    list.AddRange(GetSubCategories(cat.Categories.OrderBy(x => x.CategoryName).ToList()));
                }
                else
                {
                    list.Add(new SelectListItem()
                    {
                        Text = cat.CategoryName,
                        Value = cat.Id.ToString()
                    });
                }
            }

            
            return list;
        }
        List<SelectListItem> GetSubCategories(List<Category> categories2)
        {
            var list2 = new List<SelectListItem>();
            foreach (var cat in categories2)
            {
                if (cat.Categories.Any())
                {
                    list2.AddRange(GetSubCategories(cat.Categories.OrderBy(x => x.CategoryName).ToList()));
                }
                else
                {
                    list2.Add(new SelectListItem()
                    {
                        Text = cat.CategoryName,
                        Value = cat.Id.ToString()
                    });
                }
            }
            return list2;
        }
    }
}