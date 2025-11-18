using System;

namespace KN_ProyectoWeb.Models
{
    public class User
    {
        public String Id { get; set; }
        public String Name { get; set; }
        public String Email { get; set; }
        public String Password { get; set; }
        public String PerfilName { get; set; }
        public int ConsecutiveUser { get; set; }
        public String ConfirmPassword { get; set; }
    }
}