using E_Commerce.MvcWebUI.Entity;
using E_Commerce.MvcWebUI.Managing;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.MvcWebUI.Controllers
{
    //Sepet
    public class CartController : Controller
    {
        DbContext dbcontext;

        public CartController()
        {
            dbcontext = new DbContext();
        }
        public async Task<IActionResult> Index()
        {
            var cart = await dbcontext.firebaseClient
              .Child("Cart")
              .OnceAsync<Cart>();
            return View(cart.ToList());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Cart cart)
        {
            string guid = Guid.NewGuid().ToString();
            cart.Id = guid;
            string jsonString = JsonSerializer.Serialize(cart);
            var result = await dbcontext.firebaseClient
           .Child("Cart")
           .PostAsync(jsonString);
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var cart = await dbcontext.firebaseClient
               .Child("Cart")
               .Child(id)
               .OnceAsync<Category>();
            return View(cart.FirstOrDefault());
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Cart cart)
        {
            string jsonString = JsonSerializer.Serialize(cart);
            await dbcontext.firebaseClient
           .Child("Cart")
           .Child(cart.Id)
           .PutAsync(jsonString);
            return View();
        } 
        public async Task<IActionResult> Delete(string id)
        {
            await dbcontext.firebaseClient
            .Child("Cart")
            .Child(id)
            .DeleteAsync();
            return View();
        }
    }
}
