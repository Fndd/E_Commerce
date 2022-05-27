using E_Commerce.MvcWebUI.Entity;
using Microsoft.AspNetCore.Mvc;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using E_Commerce.MvcWebUI.Managing;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class CategoryController : Controller
    {
        DbContext dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ImageClass newImage;
        public CategoryController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            dbcontext = new DbContext();
            newImage = new ImageClass();

        }
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
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
            else
                return RedirectToAction("SignIn","Account");
        }
        public IActionResult Add()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(Category category, IFormFile file)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Category");
            category.ImagePath = newImage.UploadImage(file, uploadsFolder);

            PushResponse responses = await dbcontext.client.PushAsync("Category/", category);
            category.Id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("Category/" + category.Id, category);
            return RedirectToAction("Index", "Category");
        }

        public IActionResult Edit(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {

                FirebaseResponse response = dbcontext.client.Get("Category");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Category>();
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
                }
                if (!String.IsNullOrWhiteSpace(id))
                {
                    return View(list.Where(x => x.Id==id).FirstOrDefault());
                }
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Category category, IFormFile file, string ImagePath)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Category"); 
            category.ImagePath = newImage.UpdateImage(ImagePath, file, uploadsFolder);

            FirebaseResponse setResponse = await dbcontext.client.UpdateAsync("Category/"+category.Id, category);
            Category setcategory= setResponse.ResultAs<Category>();
            return View(setcategory);
        }
        
        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {
                FirebaseResponse response = await dbcontext.client.DeleteAsync("Category/" + id);
                Console.WriteLine(response.StatusCode);
                return RedirectToAction("Index", "Category");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            } 
        }
    }
}
