using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity; 
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using E_Commerce.MvcWebUI.Managing;

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
        }
        public async Task<IActionResult> Index()
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
        public IActionResult Add()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> Add(Product product, IFormFile file)
        {
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Product");
            product.Image = newImage.UploadImage(file, uploadsFolder);

            PushResponse responses = await dbcontext.client.PushAsync("Product/", product);
            product.Id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("Product/" + product.Id, product);
            return View();
        }

        public IActionResult Edit(string id)
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Edit(Product product, IFormFile file)
        {
            //Ürün fotografının kaydedilmesi
            string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "Image\\Product");
            product.Image = newImage.UploadImage(file, uploadsFolder);

            FirebaseResponse setResponse = await dbcontext.client.UpdateAsync("Product/" + product.Id, product);
            Product setproduct = setResponse.ResultAs<Product>();
            return View(setproduct);
        }

        public async Task<IActionResult> Delete(string id)
        {
            FirebaseResponse response = await dbcontext.client.DeleteAsync("Product/" + id);
            Console.WriteLine(response.StatusCode);
            return View();
        }

     
    }
}
