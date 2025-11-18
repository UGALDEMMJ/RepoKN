using KN_ProyectoWeb.EF;
using KN_ProyectoWeb.Models;
using KN_ProyectoWeb.Services;
using System;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace KN_ProyectoWeb.Controllers
{
    public class HomeController : Controller
    {
        Utilities utilities = new Utilities();
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
            using (var context = new BD_KNEntities1())
            {

                var result = context.tbUser.Where(x => x.Email == user.Email && x.Password == user.Password && x.State == true).FirstOrDefault();
                if (result != null)
                {
                    Session["ConsecutiveUser"] = result.ConsecutiveUser;
                    Session["Username"] = result.Name;
                    Session["Perfil"] = result.tbPerfil.Name;
                    Session["ConsecutivePerfil"] = result.ConsecutivePerfil;
                    return RedirectToAction("Principal", "Home");
                }

                ViewBag.Message = "Credentials are incorrect";
                return View();
            }
        }
        #endregion
        #region SignUp
        [HttpGet]
        public ActionResult SignUp()
        {
            return View();
        }

        [HttpPost]
        public ActionResult SignUp(User user)
        {
            using (var context = new BD_KNEntities1())
            {
                var ConsultResult = context.tbUser.Where(x => x.Id == user.Id || x.Email == user.Email).FirstOrDefault();

                if (ConsultResult == null)
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
                    var insertionResult = context.SaveChanges();

                    if (insertionResult > 0)
                    {
                        return RedirectToAction("Index", "home");
                    }
                }
                ViewBag.Message = "Information couldn't be registered";
                return View();
            }

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
            using (var context = new BD_KNEntities1())
            {
                var consultResult = context.tbUser.Where(x => x.Email == user.Email).FirstOrDefault();

                if (consultResult != null)
                {
                    var newPassword = utilities.GeneratePassword();

                    consultResult.Password = newPassword;
                    var updateResult = context.SaveChanges();

                    if (updateResult > 0)
                    {
                        string projectRoot = AppDomain.CurrentDomain.BaseDirectory;
                        string path = Path.Combine(projectRoot, "ForgotPasswordTemplate.html");

                        string htmlTemplate = System.IO.File.ReadAllText(path);

                        string message = htmlTemplate
                            .Replace("{{UserName}}", consultResult.Name)
                            .Replace("{{NewPassword}}", newPassword)
                            .Replace("{{Date}}", DateTime.Now.ToString("dd/MM/yyyy"));
                        utilities.SendEmail("New Password", message.ToString(), consultResult.Email);
                        return RedirectToAction("Index", "Home");
                    }
                }
                ViewBag.Message = "Information couldnt be restablished";
                return View();
            }
        }
        #endregion
        #region Dashboard
        [Security]
        [HttpGet]
        public ActionResult Principal()
        {
            return View();
        }
        #endregion
        #region LogOut
        [Security]
        [HttpGet]
        public ActionResult LogOut()
        {
            Session.Clear();
            Session.Abandon();
            return RedirectToAction("Index", "Home");
        }
        #endregion
    }
}