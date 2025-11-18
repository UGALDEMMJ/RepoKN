using KN_ProyectoWeb.EF;
using KN_ProyectoWeb.Models;
using KN_ProyectoWeb.Services;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.IO;
using System.Linq;
using System.Security.Permissions;
using System.Web;
using System.Web.Mvc;

namespace KN_ProyectoWeb.Controllers
{
    [Security]
    public class ProductsController : Controller
    {
        [HttpGet]
        public ActionResult GetProducts()
        {
            var results = AllProducts();
            return View(results);
        }

        #region AgregarProductos

        [HttpGet]
        public ActionResult AddProducts()
        {
            getCategory();
            return View();
        }

        [HttpPost]
        public ActionResult AddProducts(Product product, HttpPostedFileBase ImageP)
        {
            using (var context = new BD_KNEntities1())
            {
                var newProduct = new tbProduct
                {
                    ConsecutiveProduct = 0,
                    Name = product.Name,
                    Description = product.Description,
                    Price = product.Price,
                    //Quantity = product.Quantity,
                    ConsecutiveCategory = product.ConsecutiveCategory,
                    State = true,
                    ImageUrl = string.Empty
                };

                context.tbProduct.Add(newProduct);
                var result = context.SaveChanges();

                if (result > 0)
                {
                    //Guardar la imagen
                    var ext = Path.GetExtension(ImageP.FileName);
                    var ruta = AppDomain.CurrentDomain.BaseDirectory + "ImgProductos\\" + newProduct.ConsecutiveProduct + ext;
                    ImageP.SaveAs(ruta);

                    //Actualizar la ruta de la imagen
                    newProduct.ImageUrl = "/ImgProducts/" + newProduct.ConsecutiveProduct + ext;
                    context.SaveChanges();

                    return RedirectToAction("GetProductos", "Products");
                }
            }

            getCategory();
            ViewBag.Mensaje = "Error inserting the data";
            return View();
        }

        #endregion

        #region ActualizarProductos

        [HttpGet]
        public ActionResult UpdateProducts(int q)
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbProduct.Where(x => x.ConsecutiveProduct == q).ToList();

                //Convertirlo en un objeto Propio
                var data = result.Select(p => new Product
                {
                    ConsecutiveProduct = p.ConsecutiveProduct,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    //Quantity = p.Quantity,
                    ConsecutiveCategory = p.ConsecutiveCategory,
                    ImageUrl = p.ImageUrl
                }).FirstOrDefault();

                getCategory();
                return View(data);
            }
        }

        [HttpPost]
        public ActionResult UpdateProducts(Product product, HttpPostedFileBase ImgP)
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbProduct.Where(x => x.ConsecutiveProduct == product.ConsecutiveProduct).FirstOrDefault();

                //Si existe se manda a actualizar
                if (result != null)
                {
                    //Actualizar los campos del formulario
                    result.Name = product.Name;
                    result.Description = product.Description;
                    result.Price = product.Price;
                    //result.Quantity = product.Quantity;
                    result.ConsecutiveCategory = product.ConsecutiveCategory;

                    context.Entry(result).State = EntityState.Modified;
                    var updateResult = context.SaveChanges();

                    if (ImgP != null)
                    {
                        //Guardar la imagen
                        var ext = Path.GetExtension(ImgP.FileName);
                        var ruta = AppDomain.CurrentDomain.BaseDirectory + "ImgP\\" + product.ConsecutiveProduct + ext;
                        ImgP.SaveAs(ruta);
                    }

                    if (updateResult > 0)
                        return RedirectToAction("GetProducts", "Products");

                }

                getCategory();
                ViewBag.Mensaje = "Error Couldnt Update";
                return View(product);
            }
        }

        #endregion

        [HttpGet]
        public ActionResult UpdateProductState(int q)
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbProduct.Where(x => x.ConsecutiveProduct == q).FirstOrDefault();

                //Si existe se manda a actualizar
                if (result != null)
                {
                    //Elimino
                    //context.tbProducto.Remove(resultadoConsulta);

                    //Inactivando
                    result.State = result.State ? false : true;

                    var updateResult = context.SaveChanges();

                    if (updateResult > 0)
                        return RedirectToAction("GetProducts", "Products");
                }

                var endResult = AllProducts();
                ViewBag.Mensaje = "Error Couldnt Update";
                return View("GetProducts", endResult);
            }
        }

        private void getCategory()
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbCategory.ToList();

                //Convertirlo en un objeto SelectListItem
                var data = result.Select(c => new SelectListItem
                {
                    Value = c.ConsecutiveCategory.ToString(),
                    Text = c.Name
                }).ToList();

                data.Insert(0, new SelectListItem
                {
                    Value = string.Empty,
                    Text = "Select"
                });

                ViewBag.CategoryList = data;
            }
        }

        private List<Product> AllProducts()
        {
            using (var context = new BD_KNEntities1())
            {
                //Tomar el objeto de la BD
                var result = context.tbProduct.Include("tbCategory").ToList();

                //Convertirlo en un objeto Propio
                var data = result.Select(p => new Product
                {
                    ConsecutiveProduct = p.ConsecutiveProduct,
                    Name = p.Name,
                    Description = p.Description,
                    Price = p.Price,
                    //Quantity = p.Quantity,
                    ConsecutiveCategory = p.ConsecutiveCategory,
                    ImageUrl = p.ImageUrl
                }).ToList();

                return data;
            }
        }
    }
}