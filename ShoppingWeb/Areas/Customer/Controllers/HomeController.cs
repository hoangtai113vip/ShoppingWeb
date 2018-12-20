using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ShoppingWeb.Data;
using ShoppingWeb.Extensions;
using ShoppingWeb.Models;
using ShoppingWeb.Models.ViewModel;

namespace ShoppingWeb.Controllers
{
    [Area("Customer")]
    public class HomeController : Controller
    {
        private readonly ApplicationDbContext _db;
        public HomeViewModel HomeVM { get; set; }
        public HomeController( ApplicationDbContext db)
        {
            _db = db;
            HomeVM = new HomeViewModel
            {
                Products = new Products(),
                CartItem = new CartItem()
            };
        }
        public async Task<IActionResult> Index()
        {

            var productList = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).ToListAsync();

            return View(productList);
        }

        public async Task<IActionResult> Details(int id)
        {
            HomeVM.Products = await _db.Products.Include(m => m.ProductTypes).Include(m => m.SpecialTags).Where(m => m.Id == id).FirstOrDefaultAsync();

            return View(HomeVM);
        }

        [HttpPost, ActionName("Details")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DetailsPost(int id,HomeViewModel homeVM)
        {
            homeVM.CartItem.ProductName = homeVM.Products.Name;
            List<CartItem> lstShoppingCart = HttpContext.Session.Get<List<CartItem>>("ssShoppingCart");
            if (lstShoppingCart == null)
            {
                lstShoppingCart = new List<CartItem>();
            }
            lstShoppingCart.Add(homeVM.CartItem);
            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction("Index", "Home", new { area = "Customer" });
            //List<int> lstShoppingCart = HttpContext.Session.Get<List<int>>("ssShoppingCart");
            //if (lstShoppingCart == null)
            //{
            //    lstShoppingCart = new List<int>();
            //}
            //lstShoppingCart.Add(id);
            //HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            //return RedirectToAction("Index", "Home", new { area = "Customer" });

        }

        public IActionResult Remove(int id)
        {
            List<CartItem> lstShoppingCart = HttpContext.Session.Get<List<CartItem>>("ssShoppingCart");
            if (lstShoppingCart.Count > 0)
            {
                List<int> arr = new List<int>();
                foreach(var item in lstShoppingCart)
                {
                    arr.Add(item.ProductId);
                }
                if (arr.Contains(id))
                {
                    var product = lstShoppingCart.Where(m => m.ProductId == id).FirstOrDefault();
                    lstShoppingCart.Remove(product);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstShoppingCart);

            return RedirectToAction(nameof(Index));
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }


    }
}
