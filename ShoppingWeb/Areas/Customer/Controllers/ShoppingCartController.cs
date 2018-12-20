using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingWeb.Data;
using ShoppingWeb.Extensions;
using ShoppingWeb.Models;
using ShoppingWeb.Models.ViewModel;

namespace ShoppingWeb.Areas.Customer.Controllers
{
    [Area("Customer")]
    public class ShoppingCartController : Controller
    {
        private readonly ApplicationDbContext _db;

        [BindProperty]
        public ShoppingCartViewModel ShoppingCartVM { get; set; }

        public ShoppingCartController(ApplicationDbContext db)
        {
            _db = db;
            ShoppingCartVM = new ShoppingCartViewModel()
            {
                CartItems = new List<Models.CartItem>()
            };
        }

        //Get Index Shopping Cart
        public async Task<IActionResult> Index()
        {
            List<CartItem> lstShoppingCart = HttpContext.Session.Get<List<CartItem>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                foreach (var cartItem in lstShoppingCart)
                {
                    //Products prod = _db.Products.Include(p => p.SpecialTags).Include(p => p.ProductTypes).Where(p => p.Id == cartItem).FirstOrDefault();
                    ShoppingCartVM.CartItems.Add(cartItem);
                }
                
            }
            return View(ShoppingCartVM);
        }
    }
}