using E_Commerce.MvcWebUI.Models;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using E_Commerce.MvcWebUI.Entity;
 

namespace E_Commerce.MvcWebUI.Controllers
{
    public class HomeController : Controller
    {
        DbContext dbcontext;

        public HomeController()
        {
            dbcontext = new DbContext();
        }
        public IActionResult Index()
        {
            return View();  
        }

        public IActionResult Urunler(string categoryid)
        {  
            FirebaseResponse response = dbcontext.client.Get("Product");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
            }
            if (!String.IsNullOrWhiteSpace(categoryid))
            {
                return View(list.Where(x => x.CategoryId == categoryid).ToList());
            }

            return View(list);
        }
        [HttpPost]
        public IActionResult Urunler(string categoryid,string urunAdi)
        {
            FirebaseResponse response = dbcontext.client.Get("Product");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
            }
            if (!String.IsNullOrWhiteSpace(urunAdi))
            {
                return View(list.Where(x => x.Name.ToLower().Contains(urunAdi.ToLower())).ToList());
            }

            return View(list);
        }

        public IActionResult UrunDetay(string id)
        {
            FirebaseResponse response = dbcontext.client.Get("Product/" + id);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);
            return View(data);
        }
        /// <summary>
        /// Favori ürünleri listeler
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IActionResult> FavoriUrunler()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var id = HttpContext.Session.GetString("_UserId");

            if (token != null)
            {
                FirebaseResponse response = await dbcontext.client.GetAsync("User/" + id + "/FavoriteProducts/");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Product>();
                if (data != null)
                { 
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
          
           // return View();
        }
   
        /// <summary>
        /// Kullanıcı için favori ürün ekliyor
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
      
        public async Task<IActionResult> FavoriUrunEkle(string ProductId)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");

            if (token != null) {


                FirebaseResponse responsefav = await dbcontext.client.GetAsync("User/" + userid + "/FavoriteProducts/");
                dynamic datafav = JsonConvert.DeserializeObject<dynamic>(responsefav.Body);

                if (datafav == null)
                { 
                    FirebaseResponse response = dbcontext.client.Get("Product/" + ProductId);
                    Product data = JsonConvert.DeserializeObject<Product>(response.Body);

                    PushResponse responses = await dbcontext.client.PushAsync("User/" + userid + "/FavoriteProducts/", data);
                    string id = responses.Result.name;
                    SetResponse setResponse = await dbcontext.client.SetAsync("User/" + userid + "/FavoriteProducts/" + id, data);
                    return RedirectToAction("FavoriUrunler", "Home");
                }
                return View();
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
        }
        /// <summary>
        /// Kullanıcının favori ürünü silinir.
        /// </summary>
        /// <param name="id">Favori ürün Id'sidir.</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> FavoriUrunSil(string ProductId)
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");

            if (token != null)
            {
                FirebaseResponse response = await dbcontext.client.DeleteAsync("User/" + userid + "/FavoriteProducts/" + ProductId);
                return RedirectToAction("FavoriUrunler","Home");  
            }
            else
            {
                return RedirectToAction("SignIn", "Account");
            }
}

        public IActionResult Indirim()
        {
            FirebaseResponse response = dbcontext.client.Get("Product");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
            }
         return View(list.Where(x => x.Discount>0).ToList());  
        }
    
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 