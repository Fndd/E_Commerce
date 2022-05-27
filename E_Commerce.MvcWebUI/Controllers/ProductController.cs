using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity; 
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using E_Commerce.MvcWebUI.Managing;
using E_Commerce.MvcWebUI.Models;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class ProductController : Controller
    {
        DbContext dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ImageClass newImage;
        public ProductController(IWebHostEnvironment webHostEnvironment)
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
                FirebaseResponse response = await dbcontext.client.GetAsync("Product");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Product>();
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
                }
                return View(list);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        public IActionResult Add()
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
                ProductCreateModel model = new ProductCreateModel();
                model.Categories = list;

                return View(model);
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile file)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Product");
            product.Image = newImage.UploadImage(file, uploadsFolder);

            PushResponse responses = await dbcontext.client.PushAsync("Product/", product);
            product.Id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("Product/" + product.Id, product);
            return RedirectToAction("Index", "Product");
        }

        public IActionResult Edit(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {

                FirebaseResponse response = dbcontext.client.Get("Product");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Product>();
                foreach (var item in data)
                {
                    list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
                }
                if (!String.IsNullOrWhiteSpace(id))
                { 
                    FirebaseResponse responsecat = dbcontext.client.Get("Category");
                    dynamic datacat = JsonConvert.DeserializeObject<dynamic>(response.Body);
                    var listcat = new List<Category>();
                    foreach (var item in data)
                    {
                        listcat.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
                    }
                    ProductCreateModel model = new ProductCreateModel();
                    model.Categories = listcat;
                    model.Product = list.Where(x => x.Id == id).FirstOrDefault();

                    return View(model);
                }
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile file,string ImagePath)
        {
            //Ürün fotografının kaydedilmesi
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Product");
            product.Image = newImage.UpdateImage(ImagePath,file, uploadsFolder);

            FirebaseResponse setResponse = await dbcontext.client.UpdateAsync("Product/" + product.Id, product);
            Product setproduct = setResponse.ResultAs<Product>();
            return View(setproduct);
        }

        public async Task<IActionResult> Delete(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {
                FirebaseResponse response = await dbcontext.client.DeleteAsync("Product/" + id);
                Console.WriteLine(response.StatusCode);
                return RedirectToAction("Index", "Product");
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            } 
        }  
    }
}
