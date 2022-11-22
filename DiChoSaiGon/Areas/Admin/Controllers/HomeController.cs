using AspNetCoreHero.ToastNotification.Abstractions;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace DiChoSaiGon.Areas.Admin.Controllers
{
    [Area("Admin")]
    //[Route("admin.html", Name = "AdminIndex")]
    //[Authorize]
    public class HomeController : Controller
    {
        public INotyfService _notyfService { get; }
        public HomeController(INotyfService notyfService)
        {
            _notyfService = notyfService;
        }


        public IActionResult Index()
        {
            return View();
        }
    }
}
