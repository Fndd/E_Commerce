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
            //var token = HttpContext.Session.GetString("_UserToken");

            //if (token != null)
            //{
            return View();
            //}
            //else
            //{
            //    return RedirectToAction("SignIn","Account");
            //}  
        }

        public IActionResult Urunler(string categoryid)
        {
            //TODO: Örnek ürün ekleme
            //Product product = new Product();
            //product.Name = "Apple iPhone SE 64GB Siyah";
            //product.Description = "Cep Telefonu Aksesuarsız Kutu AP-IPHOSE-2020";
            //product.CategoryId = "-N014RRsjLRDaVunbnKl";
            //product.Image = "image/1_org_zoom.jpg";
            //product.Price = 7.312;


            //Product product2 = new Product();
            //product2.Name = "Philips Powerpro  Elektrikli Süpürge";
            //product2.CategoryId = "-N014piIu-1n5BBnv34Q";
            //product2.Description = "Compact Toz Torbasız";
            //product2.Image = "image/2_org_zoom.jpg";
            //product2.Price = 1.146;

            ////TODO: Push metodu ile ürün ekleme ve ıd düzenleme işlemi
            ///
            //var datas = product;
            //PushResponse responses = dbcontext.client.Push("Product/", datas);
            //datas.Id = responses.Result.name;
            //SetResponse setResponse = dbcontext.client.Set("Product/" + datas.Id, datas);

            //var datas2 = product2;
            //PushResponse responses2 = dbcontext.client.Push("Product/", datas2);
            //datas2.Id = responses2.Result.name;
            //SetResponse setResponse2 = dbcontext.client.Set("Product/" + datas2.Id, datas2);


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
        public async Task<IActionResult> FavoriUrunler(string UserId)
        {
            FirebaseResponse response = await dbcontext.client.GetAsync("User/" + UserId + "/FavoriteProducts/");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
   
        /// <summary>
        /// Kullanıcı için favori ürün ekliyor
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpPost]
        public async Task<IActionResult> FavoriUrunEkle(string ProductId, string UserId)
        {
            FirebaseResponse response = dbcontext.client.Get("Product/" + ProductId);
            Product data = JsonConvert.DeserializeObject<Product>(response.Body);

            PushResponse responses = await dbcontext.client.PushAsync("User/" + UserId + "/FavoriteProducts/", data);
            string id = responses.Result.name;
            SetResponse setResponse = await dbcontext.client.SetAsync("User/" + UserId + "/FavoriteProducts/" + id, data);
            return View();
        }
        /// <summary>
        /// Kullanıcının favori ürünü silinir.
        /// </summary>
        /// <param name="id">Favori ürün Id'sidir.</param>
        /// <param name="UserId"></param>
        /// <returns></returns>
        [HttpDelete]
        public async Task<IActionResult> FavoriUrunSil(string id, string UserId)
        {
            FirebaseResponse response = await dbcontext.client.DeleteAsync("User/" + UserId + "/FavoriteProducts/" + id);
            return View();
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
        public IActionResult Sepet()
        {
            return View();
        }
       
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
} 