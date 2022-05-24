using Microsoft.AspNetCore.Mvc;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class AdminController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
    }
}
