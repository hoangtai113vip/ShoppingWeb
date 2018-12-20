using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.Models.ViewModel
{
    public class ShoppingCartViewModel
    {
        public List<CartItem> CartItems { get; set; }
        public Appointments Appointments { get; set; }
    }
}
