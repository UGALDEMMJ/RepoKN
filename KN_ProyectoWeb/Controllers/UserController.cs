using KN_ProyectoWeb.EF;
using KN_ProyectoWeb.Models;
using KN_ProyectoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;

namespace KN_ProyectoWeb.Controllers
{
    [Security]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult GetPerfil()
        {
            using (var context = new BD_KNEntities1())
            {
                var consecutive = int.Parse(Session["ConsecutiveUser"].ToString());

                //Tomar el objeto de la BD
                var resultado = context.tbUser.Include("tbPerfil").Where(x => x.ConsecutiveUser == consecutive).ToList();

                //Convertirlo en un objeto Propio
                var datos = resultado.Select(p => new User
                {
                    Id = p.Id,
                    Name = p.Name,
                    Email = p.Email,
                    PerfilName = p.tbPerfil.Name
                }).FirstOrDefault();

                return View(datos);
            }
        }

        [HttpPost]
        public ActionResult GetPerfil(User user)
        {
            ViewBag.Mensaje = "Info update error";

            using (var context = new BD_KNEntities1())
            {
                var consecutive = int.Parse(Session["ConsecutiveUser"].ToString());

                //Tomar el objeto de la BD
                var resultadoConsulta = context.tbUser.Where(x => x.ConsecutiveUser == consecutive).FirstOrDefault();

                //Si existe se manda a actualizar
                if (resultadoConsulta != null)
                {
                    //Actualizar los campos del formulario
                    resultadoConsulta.Id = user.Id;
                    resultadoConsulta.Name = user.Name;
                    resultadoConsulta.Email = user.Email;
                    var result = context.SaveChanges();

                    if (result > 0)
                    {
                        ViewBag.Mensaje = "Succesfully Updated";
                        Session["Username"] = user.Name;
                    }
                }

                return View();
            }
        }

        [HttpGet]
        public ActionResult UpdatePassword()
        {
            return View();
        }

        [HttpPost]
        public ActionResult UpdatePassword(User user)
        {
            ViewBag.Mensaje = "Info update error";

            using (var context = new BD_KNEntities1())
            {
                var consecutive = int.Parse(Session["ConsecutiveUser"].ToString());

                //Tomar el objeto de la BD
                var result = context.tbUser.Where(x => x.ConsecutiveUser == consecutive).FirstOrDefault();

                //Si existe se manda a actualizar
                if (result != null)
                {
                    //Actualizar los campos del formulario
                    result.Password = user.Password;
                    var updateResult = context.SaveChanges();

                    if (updateResult > 0)
                        ViewBag.Mensaje = "Succesfully Updated";
                }

                return View();
            }
        }

    }
}