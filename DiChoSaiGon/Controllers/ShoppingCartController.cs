using AspNetCoreHero.ToastNotification.Abstractions;
using DiChoSaiGon.Extensions;
using DiChoSaiGon.Models;
using DiChoSaiGon.ModelViews;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiChoSaiGon.Controllers
{
    public class ShoppingCartController : Controller
    {
        private NorthwindContext _context;
        public INotyfService _notyfService { get; }
        public ShoppingCartController(NorthwindContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }        
        public List<CartItem> GioHang
        {
            get
            {
                var gioHang = HttpContext.Session.Get<List<CartItem>>("GioHang");
                if (gioHang == default(List<CartItem>))
                {
                    gioHang = new List<CartItem>();
                }
                return gioHang;
            }
        }

        [HttpPost]
        [Route("api/cart/add")]
        public IActionResult AddToCart(int productID, int? amount)
        {
            List<CartItem> cart = GioHang;
            try
            {
                //Them san pham vao gio hang
                CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    item.amount = item.amount + amount.Value;
                    //Luu lai session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                else
                {
                    Product product = _context.Products.SingleOrDefault(p => p.ProductId == productID);
                    item = new CartItem
                    {
                        amount = amount.HasValue ? amount.Value : 1,
                        product = product
                    };
                    cart.Add(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                _notyfService.Success("Thêm sản phẩm thành công");
                return Json(new { success = true });
            }
            catch 
            {
                return Json(new { success = false });
            }
        }

        [HttpPost]
        [Route("api/cart/update")]
        public IActionResult UpdateCart(int productID, int? amount)
        {
            var cart = HttpContext.Session.Get<List<CartItem>>("GioHang");
            try
            {
                if (cart != null)
                {
                    CartItem item = cart.SingleOrDefault(p => p.product.ProductId == productID);
                    if (item != null && amount.HasValue)
                    {
                        item.amount = amount.Value;
                    }
                    //Luu lai session
                    HttpContext.Session.Set<List<CartItem>>("GioHang", cart);
                }
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { succes = false });
            }
        }

        

        [HttpPost]
        [Route("api/cart/remove")]
        public ActionResult Remove(int productID)
        {
            try
            {
                List<CartItem> gioHang = GioHang;
                CartItem item = gioHang.SingleOrDefault(p => p.product.ProductId == productID);
                if (item != null)
                {
                    gioHang.Remove(item);
                }
                HttpContext.Session.Set<List<CartItem>>("GioHang", gioHang);
                return Json(new { success = true });
            }
            catch
            {
                return Json(new { success = false });
            }
            
        }

        [Route("cart.html", Name = "Cart")]
        public IActionResult Index()
        {
            return View(GioHang);
        }
    }
}
