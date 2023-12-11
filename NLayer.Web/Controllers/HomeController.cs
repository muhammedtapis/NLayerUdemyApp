using Microsoft.AspNetCore.Mvc;
using NLayer.Core.DTOs;
using System.Diagnostics;

namespace NLayer.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;

        public HomeController(ILogger<HomeController> logger)
        {
            _logger = logger;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Privacy()
        {
            return View();
        }

        //bu action metoduna bizim oluşturduğumuz viewmodeli gönderdik.Error.cshtml sayfasında bu modeli yakalayıp modeldeki Errors propertysine erişeceğiz.

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error(ErrorViewModel errorViewModel)
        {
            return View(errorViewModel);
        }
    }
}