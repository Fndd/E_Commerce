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
        public async Task<IActionResult> Sepet()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");

            if (token != null)
            {
                FirebaseResponse response = await dbcontext.client.GetAsync("User/" + userid + "/Cart/");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Cart>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Cart>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
            }
                return View();
        }

        /// <summary>
        /// Kullanıcı için sepete ürün ekliyor
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
       
        public async Task<IActionResult> SepeteUrunEkle(string ProductId)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");

            if (token != null)
            {
                //Ürünü bul
                FirebaseResponse response = dbcontext.client.Get("Product/" + ProductId);
                Product data = JsonConvert.DeserializeObject<Product>(response.Body);

                //Kullanıcının sepetinde daha önce bu ürün eklenmiş mi kontrol et
                FirebaseResponse responseSepet = dbcontext.client.Get("User/" + userid + "/Cart/");
                dynamic datasepet = JsonConvert.DeserializeObject<dynamic>(responseSepet.Body);
                var list = new List<Entity.Cart>();
                if (datasepet != null)
                {
                    foreach (var item in datasepet)
                    {
                        list.Add(JsonConvert.DeserializeObject<Entity.Cart>(((JProperty)item).Value.ToString()));
                    }
                }
                var sepetProduct = list.Where(x => x.ProductId == ProductId).FirstOrDefault();
                //varsa sadece miktar ve fiyatı güncelle
                    if (sepetProduct!=null)
                    { 
                    sepetProduct.Quantity += 1;
                    sepetProduct.RowTotal += data.Price;
                    FirebaseResponse setResponse = await dbcontext.client.UpdateAsync("User/" + userid + "/Cart/" + sepetProduct.Id, sepetProduct);
                    }
                    else
                    {
                    //yoksa yeni oluştur.
                    sepetProduct = new Entity.Cart(); 
                    sepetProduct.ProductId = data.Id;
                    sepetProduct.Name = data.Name;
                    sepetProduct.Image = data.Image;
                    sepetProduct.Discount= data.Discount;
                    sepetProduct.Price = data.Price;    
                    sepetProduct.DiscountedPrice= data.DiscountedPrice;
                    sepetProduct.Quantity += 1;
                    sepetProduct.ProductCode = data.ProductCode;
                    if (data.DiscountedPrice > 0)
                    {
                        sepetProduct.RowTotal += data.DiscountedPrice;
                    }
                    else
                    {
                        sepetProduct.RowTotal += data.Price;
                    }
                    //Yoksa yeni oluştur
                    PushResponse responses = await dbcontext.client.PushAsync("User/" + userid + "/Cart/", sepetProduct);
                    string id = responses.Result.name;
                    sepetProduct.Id = id;
                    SetResponse setResponse = await dbcontext.client.SetAsync("User/" + userid + "/Cart/" + id, sepetProduct);
                    }
                    return RedirectToAction("Sepet","Cart");
            }
            return View();
        }
        /// <summary>
        /// Kullanıcının sepetteki ürünü silinir.
        /// </summary>
        /// <param name="id">Favori ürün Id'sidir.</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
 
        public async Task<IActionResult> SepettenUrunSil(string id)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");

            if (token != null)
            {
                FirebaseResponse response = await dbcontext.client.DeleteAsync("User/" + userid + "/Cart/" + id);
                return RedirectToAction("Sepet");
            }
            return View();
        } 
    }
}
