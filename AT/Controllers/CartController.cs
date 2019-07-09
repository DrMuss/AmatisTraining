using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using AT.Data.Courses;
using Microsoft.AspNetCore.Http.Features;
using Microsoft.AspNetCore.Http;

namespace AT.Controllers
{
    public class CartController : Controller
    {
        private readonly IATRepository _repository;
        private readonly ICart _cart;


        public CartController(IATRepository rep, ICart cart)
        {
            _repository = rep;
            _cart = cart;
        }

        public IActionResult Index(string returnUrl)
        {
           
            try
            {
                return View(_cart);
            }
            catch
            {
                return RedirectToAction("Index", "Home");
            }
           
        }

        public IActionResult AddToCart(int productId, string returnurl)
        {
            Course prod = _repository.GetCourse(productId);
           
            if (prod != null)
            {
                _cart.AddItem(prod, 1);
            }
           
            return RedirectToLocal(returnurl);
        }

        public IActionResult RemoveFromCart(int productId, string returnUrl)
        {
           
            _cart.RemoveLine(productId);
            return RedirectToLocal(returnUrl);
        }

        public RedirectToActionResult ClearCart()
        {

           
            _cart.Clear();
            return RedirectToAction("Index","Home");
        }

        private IActionResult RedirectToLocal(string returnUrl)
        {
            if (Url.IsLocalUrl(returnUrl))
            {
                return Redirect(returnUrl);
            }
            else
            {
                return RedirectToAction(nameof(HomeController.Index), "Home");
            }
        }
    }
}
