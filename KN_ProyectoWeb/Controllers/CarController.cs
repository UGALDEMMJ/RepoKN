using KN_ProyectoWeb.EF;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using KN_ProyectoWeb.Models;

namespace KN_ProyectoWeb.Controllers
{
    public class CarController : Controller
    {
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
    }
}