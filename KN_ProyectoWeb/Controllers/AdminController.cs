using KN_ProyectoWeb.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace KN_ProyectoWeb.Controllers
{
    public class AdminController : Controller
    {
        [HttpGet]
        public ActionResult Principal()
        {
            if (Session["ConsecutivePerfil"].ToString() != "1")
                return RedirectToAction("Principal", "Home");

            using (var context = new BD_KNEntities1())
            {
                var usuarios = context.tbUser.Where(x => x.ConsecutivePerfil != 1).ToList();
                var productos = context.tbProduct.ToList();

                ViewBag.ActiveUsers = usuarios.Where(x => x.State== true).Count();
                ViewBag.InactiveUsers = usuarios.Where(x => x.State== false).Count();
                ViewBag.ActiveProducts = productos.Where(x => x.State == true).Count();
                ViewBag.InactiveProducts = productos.Where(x => x.State == false).Count();

                return View();
            }
        }
    }
}