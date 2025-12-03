
using KN_ProyectoWeb.EF;
using System;
using System.Configuration;
using System.Net;
using System.Net.Mail;
using System.Security.Cryptography;
using System.Web;
using System.Linq;

namespace KN_ProyectoWeb.Services
{
    public class Utilities
    {

        public string GeneratePassword()
        {
            int lenght = 8;
            const string caracteres = "ABCDEFGHIJKLMNOPQRSTUVWXYZ0123456789";
            char[] resultado = new char[lenght];

            using (var rng = RandomNumberGenerator.Create())
            {
                byte[] buffer = new byte[sizeof(uint)];

                for (int i = 0; i < lenght; i++)
                {
                    rng.GetBytes(buffer);
                    uint num = BitConverter.ToUInt32(buffer, 0);
                    resultado[i] = caracteres[(int)(num % (uint)caracteres.Length)];
                }
            }

            return new string(resultado);
        }

        public void SendEmail(string subject, string body, string userEmail)
        {
            var emailSMTP = ConfigurationManager.AppSettings["EmailSMTP"];
            var passwordSMTP = ConfigurationManager.AppSettings["PasswordSMTP"];

            if (passwordSMTP != null)
            {
                var smtp = new SmtpClient("smtp.office365.com")
                {
                    Port = 587,
                    Credentials = new NetworkCredential(emailSMTP, passwordSMTP),
                    EnableSsl = true
                };

                var message = new MailMessage
                {
                    From = new MailAddress(emailSMTP, "Marcos Ugalde"),
                    Subject = subject,
                    Body = body,
                    IsBodyHtml = true
                };

                message.To.Add(userEmail);
                smtp.Send(message);
            }
        }

        public void CalculateResumenActualCar()
        {
            using (var context = new BD_KNEntities1())
            {
                var consecutive = int.Parse(HttpContext.Current.Session["ConsecutiveUser"].ToString());

                //Tomar el objeto de la BD
                var result = context.tbCar.Include("tbProduct").Where(x => x.ConsecutiveUser == consecutive).ToList();

                var subTotal = result.Sum(x => x.tbProduct.Price * x.Quantity);

                HttpContext.Current.Session["Total"] = subTotal * 1.13M;
                HttpContext.Current.Session["Cantidad"] = result.Sum(x => x.Quantity);
            }
        }
    }
}