using E_Commerce.MvcWebUI.Entity;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

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
            FirebaseResponse response = await dbcontext.client.GetAsync("Category");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category)
        { 
            PushResponse responses = await dbcontext.client.PushAsync("Category/", category);
            category.Id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("Category/" + category.Id, category);
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(string id)
        { 
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category)
        {
            FirebaseResponse setResponse = await dbcontext.client.UpdateAsync("Category/"+category.Id, category);
            Category setcategory= setResponse.ResultAs<Category>();
            return View(setcategory);
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            FirebaseResponse response = await dbcontext.client.DeleteAsync("Category/"+id);  
            Console.WriteLine(response.StatusCode);
            return RedirectToAction("Index","Category");
        }
    }
}
