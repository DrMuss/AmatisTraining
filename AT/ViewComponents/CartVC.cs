
using AT.Data.Courses;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Arcania.ViewComponents
{
    public class CartVC : ViewComponent
    {
        private readonly ICart _cart;
        public CartVC(ICart cart)
        {
            _cart = cart;
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {

            
            return View(_cart);
        }

        
    }
}
