using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KN_ProyectoWeb.Models
{
    public class Product
    {
        public int ConsecutiveProduct { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public int ConsecutiveCategory { get; set; }
        public bool State { get; set; }
        public string ImageUrl { get; set; }
    }
}