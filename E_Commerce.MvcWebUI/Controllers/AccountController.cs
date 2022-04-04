using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity;
using E_Commerce.MvcWebUI.Managing; 
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace E_Commerce.MvcWebUI.Controllers
{
    public class AccountController : Controller
    {
        DbContext dbcontext;

        public AccountController()
        {
            dbcontext = new DbContext();
        }
        public IActionResult Register()
        { 
        return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(User user)
        {
            string guid = Guid.NewGuid().ToString();
            user.Id = guid;
            string jsonString = JsonSerializer.Serialize(user);
            var result = await dbcontext.firebaseClient
           .Child("Category")
           .PostAsync(jsonString);
            return View();
        }

    }
}
