using Microsoft.AspNetCore.Mvc;
using SistemKasir.Models;
using SistemKasir.ViewModels;
using System.Diagnostics;

namespace SistemKasir.Controllers
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
    }
}