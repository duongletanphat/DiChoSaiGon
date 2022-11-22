using DiChoSaiGon.Extensions;
using DiChoSaiGon.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiChoSaiGon.Controllers.Component
{
    public class NumberCartViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            return View(cart);
        }
    }
}
