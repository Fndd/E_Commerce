using Microsoft.AspNetCore.Mvc;
using E_Commerce.MvcWebUI.Entity;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using E_Commerce.MvcWebUI.Managing;

namespace E_Commerce.MvcWebUI.Controllers
{
    //Sepet
    public class CartController : Controller
    {
        DbContext dbcontext;
        private readonly IWebHostEnvironment _webHostEnvironment;
        ImageClass newImage;
        public CartController(IWebHostEnvironment webHostEnvironment)
        {
            _webHostEnvironment = webHostEnvironment;
            dbcontext = new DbContext();
        }
        /// <summary>
        /// Sepetteki ürünleri listeler
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Sepet(string UserId)
        {
            FirebaseResponse response = await dbcontext.client.GetAsync("User/" + UserId + "/Cart/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Cart>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Cart>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }

        /// <summary>
        /// Kullanıcı için sepete ürün ekliyor
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> SepeteUrunEkle(string ProductId, string UserId)
        { 
            FirebaseResponse response = dbcontext.client.Get("Product/" + ProductId);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);

            PushResponse responses = await dbcontext.client.PushAsync("User/" + UserId + "/Cart/", data);
            string id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("User/" + UserId + "/Cart/" + id, data);
            return View();
        }
        /// <summary>
        /// Kullanıcının sepetteki ürünü silinir.
        /// </summary>
        /// <param name="id">Favori ürün Id'sidir.</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> SepettenUrunSil(string id, string UserId)
        {
            FirebaseResponse response = await dbcontext.client.DeleteAsync("User/" + UserId + "/Cart/" + id);
            return View();
        }




    }
}
