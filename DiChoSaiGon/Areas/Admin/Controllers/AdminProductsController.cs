using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using DiChoSaiGon.Models;
using AspNetCoreHero.ToastNotification.Abstractions;
using PagedList.Core;
using DiChoSaiGon.Helpper;
using System.IO;
using Microsoft.AspNetCore.Authorization;

namespace DiChoSaiGon.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Authorize]
    public class AdminProductsController : Controller
    {
        private readonly NorthwindContext _context;
        public INotyfService _notyfService { get; }
        public AdminProductsController(NorthwindContext context, INotyfService notyfService)
        {
            _context = context;
            _notyfService = notyfService;
        }

        // GET: Admin/AdminProducts
        public IActionResult Index(int page = 1, int CatID = 0)
        {
            var pageNumber = page;
            var pageSize = 20;

            List<Product> lsProducts = new List<Product>();

            if (CatID != 0)
            {
                lsProducts = _context.Products
                    .AsNoTracking()
                    .Where(x => x.CategoryId == CatID)
                    .Include(x => x.Category)
                    .OrderByDescending(x => x.ProductId).ToList();
            }
            else
            {
                lsProducts = _context.Products
                    .AsNoTracking()
                    .Include(x => x.Category)
                    .OrderByDescending(p => p.ProductId).ToList();
            }

            PagedList<Product> models = new PagedList<Product>(lsProducts.AsQueryable(), pageNumber, pageSize);

            ViewBag.CurrentCatID = CatID;
            ViewBag.CurrentPage = pageNumber;
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");
            return View(models);
        }

        public IActionResult Filter(int CatID = 0)
        {
            var url = $"/Admin/AdminProducts?CatID={CatID}";
            if (CatID == 0)
            {
                url = $"/Admin/AdminProducts";
            }
            return Json(new { status = "success", rediectUrl = url });
        }
        // GET: Admin/AdminProducts/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // GET: Admin/AdminProducts/Create
        public IActionResult Create()
        {
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName");            
            return View();
        }

        // POST: Admin/AdminProducts/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Discount,Video,Alias,Picture,MetaKey,BestSellers,HomeFlag,Active,Tags,Title,MetaDesc")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (ModelState.IsValid)
            {
                product.ProductName = Utilities.ToTitleCase(product.ProductName);
                if (fThumb != null)
                {
                    string extension = Path.GetExtension(fThumb.FileName);
                    string image = Utilities.SEOUrl(product.ProductName) + extension;
                    product.Picture = await Utilities.UploadFile(fThumb, @"product", image.ToLower());
                }
                if (string.IsNullOrEmpty(product.Picture)) product.Picture = "default.jpg";
                product.Alias = Utilities.SEOUrl(product.ProductName);
                
                _context.Add(product);
                await _context.SaveChangesAsync();
                _notyfService.Success("Thêm sản phẩm thành công");
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            
            return View(product);
        }

        // GET: Admin/AdminProducts/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            
            return View(product);
        }

        // POST: Admin/AdminProducts/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ProductId,ProductName,SupplierId,CategoryId,QuantityPerUnit,UnitPrice,UnitsInStock,UnitsOnOrder,ReorderLevel,Discontinued,Discount,Video,Alias,Picture,MetaKey,BestSellers,HomeFlag,Active,Tags,Title,MetaDesc")] Product product, Microsoft.AspNetCore.Http.IFormFile fThumb)
        {
            if (id != product.ProductId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    product.ProductName = Utilities.ToTitleCase(product.ProductName);
                    if (fThumb != null)
                    {
                        string extension = Path.GetExtension(fThumb.FileName);
                        string image = Utilities.SEOUrl(product.ProductName) + extension;
                        product.Picture = await Utilities.UploadFile(fThumb, @"products", image.ToLower());
                    }
                    if (string.IsNullOrEmpty(product.Picture)) product.Picture = "default.jpg";
                    _context.Update(product);
                    _notyfService.Success("Cập nhật thành công");
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!ProductExists(product.ProductId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["DanhMuc"] = new SelectList(_context.Categories, "CategoryId", "CategoryName", product.CategoryId);
            
            return View(product);
        }

        // GET: Admin/AdminProducts/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .Include(p => p.Category)
                .Include(p => p.Supplier)
                .FirstOrDefaultAsync(m => m.ProductId == id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Admin/AdminProducts/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            _context.Products.Remove(product);
            await _context.SaveChangesAsync();
            _notyfService.Success("Xóa thành công");
            return RedirectToAction(nameof(Index));
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.ProductId == id);
        }
    }
}
