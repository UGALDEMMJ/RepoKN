using KN_ProyectoWeb.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KN_ProyectoWeb.Controllers
{
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult getProducts()
        {
            using (var context = new BD_KNEntities1())
            {
                var result = context.tbProduct.Include("tbCategory").ToList();
                return View(result);
            }
        }
    }
}