using KN_ProyectoWeb.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KN_ProyectoWeb.Services;
using KN_ProyectoWeb.Models;

namespace KN_ProyectoWeb.Controllers
{
    [Security]
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult GetProducts()
        {
            using (var context = new BD_KNEntities1())
            {
                var result = context.tbProduct.Include("tbCategory").ToList();
                var data = result.Select(x => new Product
                {
                    ConsecutiveProduct = x.ConsecutiveProduct,
                    Name = x.Name,
                    Price = x.Price,
                    CategoryName = x.tbCategory.Name,
                    State = x.State,
                    ImageUrl = x.ImageUrl
                }).ToList();
                return View(data);
            }
        }
    }
}