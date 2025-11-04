using KN_ProyectoWeb.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;
using System.Web.UI;
using KN_ProyectoWeb.Models;

namespace KN_ProyectoWeb.Controllers
{
    [Security]
    [OutputCache(Duration = 0, Location = OutputCacheLocation.None, NoStore = true, VaryByParam = "*")]
    public class UserController : Controller
    {
        [HttpGet]
        public ActionResult getPerfil()
        {
            using (var context = new EF.BD_KNEntities1())
            {
                var consecutiveUser = int.Parse(Session["ConsecutiveUser"].ToString());
                var result = context.tbUser.Include("tbPerfil").Where(x => x.ConsecutiveUser == consecutiveUser);

                var data = result.Select(x => new User
                {
                    ConsecutiveUser = (int)x.ConsecutiveUser,
                    Id = x.Id,
                    Name = x.Name,
                    Email = x.Email,
                    PerfilName = x.tbPerfil.Name
                }).ToList();

                return View(result);
            }
        }
    }
}