using KN_ProyectoWeb.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KN_ProyectoWeb.Models;
using System.Security.Permissions;
using KN_ProyectoWeb.Services;

namespace KN_ProyectoWeb.Controllers
{
    [Security]
    public class CarController : Controller
    {
        Utilities utilities = new Utilities();

        [HttpGet]
        public ActionResult GetCar()
        {
            using (var context = new BD_KNEntities1())
            {
                var consecutive = int.Parse(Session["ConsecutiveUser"].ToString());

                //Tomar el objeto de la BD
                var result = context.tbCar.Include("tbProduct").Where(x => x.ConsecutiveUser == consecutive).ToList();

                //Convertirlo en un objeto Propio
                var datos = result.Select(p => new Car
                {
                    ConsecutiveProduct= p.ConsecutiveProduct,
                    Name = p.tbProduct.Name,
                    Price = p.tbProduct.Price,
                    Quantity = p.Quantity,
                    SubTotal = p.tbProduct.Price * p.Quantity,
                    Tax = ((p.tbProduct.Price * p.Quantity) * 0.13M),
                    Total = ((p.tbProduct.Price * p.Quantity) * 1.13M)
                }).ToList();

                return View(datos);
            }
        }

        [HttpGet]
        public ActionResult RemoveProductCar(int q)
        {
            var consecutive = int.Parse(Session["ConsecutiveUser"].ToString());

            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbCar.Where(x => x.ConsecutiveProduct== q && x.ConsecutiveUser== consecutive).FirstOrDefault();

                //Si existe se manda a actualizar
                if (result != null)
                {
                    context.tbCar.Remove(result);
                    context.SaveChanges();
                    utilities.CalculateResumenActualCar();
                }

                return RedirectToAction("VerMiCarrito", "Carrito");
            }
        }
    }
}