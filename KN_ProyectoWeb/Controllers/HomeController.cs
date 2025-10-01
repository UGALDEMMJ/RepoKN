using KN_ProyectoWeb.EF;
using KN_ProyectoWeb.Models;
using System.Web.Mvc;

namespace KN_ProyectoWeb.Controllers
{
    public class HomeController : Controller
    {
        #region Login
        [HttpGet]
        public ActionResult Index()
        {
            return View();
        }
        //Accion que me permite ejecutar funciones del vista que se abrio
        [HttpPost]
        public ActionResult Index(User user)
        {
            //Progra para inicial sesion en la db
            return RedirectToAction("Principal", "Home");
        }
        #endregion
        #region SignUp
        [HttpGet]
        public ActionResult Registro()
        {
            return View();
        }

        [HttpPost]
        public ActionResult Registro(User user)
        {
            using(var context = new BD_KNEntities()) 
            {
                var newUser = new tbUser
                {
                    Id = user.Id,
                    Name = user.Name,
                    Email = user.Email,
                    Password = user.Password,
                    ConsecutivePerfil = 2,
                    State = true
                };

                context.tbUser.Add(newUser);
                context.SaveChanges();
            }

            return View();
        }
        #endregion
        #region Forgot Password
        [HttpGet]
        public ActionResult ForgotPassword()
        {
            return View();
        }
        [HttpPost]
        public ActionResult ForgotPassword(User user)
        {
            return View();
        }
        #endregion
        #region Dashboard
        public ActionResult Principal()
        {
            return View();
        }
        #endregion
    }
}