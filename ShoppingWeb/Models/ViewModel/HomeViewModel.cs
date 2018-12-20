using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ShoppingWeb.Models.ViewModel
{
    public class HomeViewModel
    {
        public Products Products { get; set; }
        public CartItem CartItem { get; set; }
    }
}
