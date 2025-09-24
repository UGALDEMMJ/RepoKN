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