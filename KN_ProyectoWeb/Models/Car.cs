using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace KN_ProyectoWeb.Models
{
    public class Car
    {
        public int ConsecutiveCar { get; set; }
        public int ConsecutiveProduct { get; set; }
        public int Quantity { get; set; }
        public DateTime Date { get; set; }
        public int Id { get; set; }
        public string Name { get; set; }
        public decimal Price { get; set; }
        public decimal SubTotal { get; set; }
        public decimal Tax { get; set; }
        public decimal Total { get; set; }
    }
}