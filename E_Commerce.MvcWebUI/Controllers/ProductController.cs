using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity; 
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace E_Commerce.MvcWebUI.Controllers
{
    public class ProductController : Controller
    { 
        DbContext dbcontext;
        public ProductController()
        {
            dbcontext = new DbContext();
        }
        public async Task<IActionResult> Index()
        {
            var products = await dbcontext.firebaseClient
              .Child("Product")
              .OnceAsync<Product>();
            return View(products.ToList());
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product)
        {
            string guid = Guid.NewGuid().ToString();    
            product.Id= guid;
            string jsonString = JsonSerializer.Serialize(product);
            var result = await dbcontext.firebaseClient
           .Child("Product")
           .PostAsync(jsonString);
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            var category = await dbcontext.firebaseClient
               .Child("Product")
               .Child(id)
               .OnceAsync<Product>();
            return View(category.FirstOrDefault());
        }
         [HttpPost]
        public async Task<IActionResult> Edit(Product product)
        {
            string jsonString = JsonSerializer.Serialize(product);
            await dbcontext.firebaseClient
           .Child("Product")
           .Child(product.Id)
           .PutAsync(jsonString);
            return View();
        }
     
        public async Task<IActionResult> Delete(string id)
        {
            await dbcontext.firebaseClient
            .Child("Product")
            .Child(id)
            .DeleteAsync();
            return View();
        }
    }
}
