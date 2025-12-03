using KN_ProyectoWeb.EF;
using KN_ProyectoWeb.Models;
using KN_ProyectoWeb.Services;
using System;
using System.Collections.Generic;
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
            if (Session["ConsecutivePerfil"].ToString() == "1")
                return RedirectToAction("Principal", "Admin");

            utilities.CalculateResumenActualCar();

            var data = ConsultSellData();
            return View(data);
        }
        #endregion
        [Security]
        [HttpPost]
        public ActionResult AddProductCar(Product product)
        {
            using (var context = new BD_KNEntities1())
            {
                var availability = context.tbProduct.Where(x => x.ConsecutiveProduct == product.ConsecutiveProduct).FirstOrDefault();

                if (product.Quantity > availability.Quantity)
                {
                    ViewBag.Mensaje = "We dont stock for those items, products in stock: " + availability.Quantity;
                    return View("Principal", ConsultSellData());
                }

                var consecutiveUser = int.Parse(Session["ConsecutiveUser"].ToString());
                var result = context.tbCar.Where(x => x.ConsecutiveProduct== product.ConsecutiveProduct 
                && x.ConsecutiveUser == consecutiveUser).FirstOrDefault();

                if (result == null)
                {
                    var newCar = new tbCar
                    {
                        ConsecutiveUser = consecutiveUser,
                        ConsecutiveProduct = product.ConsecutiveProduct,
                        Quantity = product.Quantity.Value,
                        Date = DateTime.Now
                    };

                    context.tbCar.Add(newCar);
                    context.SaveChanges();
                }
                else
                {
                    result.Quantity= product.Quantity.Value;
                    result.Date = DateTime.Now;
                    context.SaveChanges();
                }

                return RedirectToAction("Principal", "Home");
            }
        }

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

        private List<Product> ConsultSellData()
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbProduct.Include("tbCategory")
                    .Where(x => x.State == true
                        && x.Quantity > 0).ToList();

                //Convertirlo en un objeto Propio
                var datos = result.Select(p => new Product
                {
                    ConsecutiveProduct = p.ConsecutiveProduct,
                    Name = p.Name,
                    Price = p.Price,
                    Quantity = p.Quantity,
                    CategoryName = p.tbCategory.Name,
                    State= p.State,
                    ImageUrl = p.ImageUrl
                }).ToList();

                return datos;
            }
        }
    }
}