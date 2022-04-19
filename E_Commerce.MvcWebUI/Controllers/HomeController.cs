using E_Commerce.MvcWebUI.Models;
using Firebase.Auth;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class HomeController : Controller
    {  
        
        public HomeController()
        {
            
        }  
        public IActionResult Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            if (token != null)
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn","Account");
            }
        }

        public IActionResult Urunler()
        {
            return View();
        }        
        public IActionResult UrunDetay()
        {
            return View();
        }
        public IActionResult Indirim()
        {
            return View();
        }
        public IActionResult Sepet()
        {
            return View();
        }
        public IActionResult Login()
        {
            return View();
        }
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 