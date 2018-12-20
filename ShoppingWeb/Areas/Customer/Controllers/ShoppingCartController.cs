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


        [HttpPost]
        [ValidateAntiForgeryToken]
        [ActionName("Index")]
        public IActionResult IndexPost()
        {
            List<CartItem> lstCartItems = HttpContext.Session.Get<List<CartItem>>("ssShoppingCart");

            ShoppingCartVM.Appointments.AppointmentDate = ShoppingCartVM.Appointments.AppointmentDate
                                                           .AddHours(ShoppingCartVM.Appointments.AppointmentTime.Hour)
                                                           .AddMinutes(ShoppingCartVM.Appointments.AppointmentTime.Minute);

            Appointments appointments = ShoppingCartVM.Appointments;
            _db.Appointments.Add(appointments);
            _db.SaveChanges();

            int appointmentId = appointments.Id;

            foreach (var cartItem in lstCartItems)
            {
                ProductsSelectedForAppointment productsSelectedForAppointment = new ProductsSelectedForAppointment()
                {
                    AppointmentId = appointmentId,
                    ProductId = cartItem.ProductId,
                    Quatity =cartItem.Quatity
                };
                _db.ProductsSelectedForAppointments.Add(productsSelectedForAppointment);

            }
            _db.SaveChanges();
                lstCartItems = new List<CartItem>();
                HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction("Index");

        }
        // remove product in cart
        public IActionResult Remove(int id)
        {
            List<CartItem> lstCartItems = HttpContext.Session.Get<List<CartItem>>("ssShoppingCart");

            if (lstCartItems.Count > 0)
            {
                List<int> arr = new List<int>();
                foreach(var item in lstCartItems)
                {
                    arr.Add(item.ProductId);

                }
                if (arr.Contains(id))
                {
                    var cartItem = lstCartItems.Where(m => m.ProductId == id).FirstOrDefault();
                    lstCartItems.Remove(cartItem);
                }
            }

            HttpContext.Session.Set("ssShoppingCart", lstCartItems);

            return RedirectToAction(nameof(Index));
        }


    }
}