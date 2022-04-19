using E_Commerce.MvcWebUI.Entity;
using Microsoft.AspNetCore.Mvc;
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class CategoryController : Controller
    {
        DbContext dbcontext;

        public CategoryController()
        {
            dbcontext = new DbContext();
        }
        public async Task<IActionResult> Index()
        {
            //var categories = await dbcontext.firebaseClient
            //  .Child("Category")
            //  .OnceAsync<Category>();
            //return View(categories.ToList());
            return View();  
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        {
           // string guid = Guid.NewGuid().ToString();
           // category.Id = guid;
           // string jsonString = JsonSerializer.Serialize(category);
           // var result = await dbcontext.firebaseClient
           //.Child("Category")
           //.PostAsync(jsonString);
            return View();
        }

        public async Task<IActionResult> Edit(string id)
        {
            //var category = await dbcontext.firebaseClient
            //   .Child("Category")
            //   .Child(id)
            //   .OnceAsync<Category>();
            //return View(category.FirstOrDefault());
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
           // string jsonString = JsonSerializer.Serialize(category);
           // await dbcontext.firebaseClient
           //.Child("Category")
           //.Child(category.Id)
           //.PutAsync(jsonString);
            return View();
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            //await dbcontext.firebaseClient
            //.Child("Categry")
            //.Child(id)
            //.DeleteAsync();
            return View();
        }
    }
}
