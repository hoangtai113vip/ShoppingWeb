using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.Models
{
    public class CartItem
    {
        public int ProductId { get; set; }
        public int Quanlity { get; set; }
        public decimal UnitPrice { get; set; }
    }
}
