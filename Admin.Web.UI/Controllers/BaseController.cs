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
            var categories = new CategoryRepo().GetAll(x=>x.SupCategoryID==null).OrderBy(x => x.CategoryName);
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
                    list.Add(new SelectListItem()
                    {
                        Text = cat.CategoryName,
                        Value = cat.Id.ToString()
                    });
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
                    list2.Add(new SelectListItem()
                    {
                        Text = cat.CategoryName,
                        Value = cat.Id.ToString()
                    });
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

        protected List<SelectListItem> GetProductSelectList()
        {
            var products = new ProductRepo().GetAll(x => x.SupProductID == null).OrderBy(x => x.ProductName).ToList();
            List<SelectListItem> prdList = new List<SelectListItem>()
            {
                new SelectListItem()
                {
                    Text = "Üst ürünü yok",
                    Value = "null"
                }
            };

            foreach (var prd in products)
            {
                if (prd.Products.Any())
                {
                    prdList.Add(new SelectListItem() {
                        Text = prd.ProductName,
                        Value = prd.Id.ToString()
                    });
                    prdList.AddRange(GetSubProducts(prd.Products.OrderBy(x => x.ProductName).ToList()));
                }
                else
                {
                    prdList.Add(new SelectListItem()
                    {
                        Text = prd.ProductName,
                        Value = prd.Id.ToString()
                    });
                }
            }

            return prdList;
        }

        private IEnumerable<SelectListItem> GetSubProducts(List<Product> products)
        {
            List<SelectListItem> prdList2 = new List<SelectListItem>();
            foreach (var prd in products)
            {
                if (prd.Products.Any())
                {
                    prdList2.Add(new SelectListItem()
                    {
                        Text = prd.ProductName,
                        Value = prd.Id.ToString()
                    });
                    prdList2.AddRange(GetSubProducts(prd.Products.OrderBy(x => x.ProductName).ToList()));
                }
                else
                {
                    prdList2.Add(new SelectListItem()
                    {
                        Text = prd.ProductName,
                        Value = prd.Id.ToString()
                    });
                }
            }

            return prdList2;
        }
    }
}