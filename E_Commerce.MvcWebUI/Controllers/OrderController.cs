using E_Commerce.MvcWebUI.Entity;
using FireSharp.Response;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class OrderController : Controller
    {
        DbContext dbcontext;

        public OrderController()
        {
            dbcontext = new DbContext();
        }
        /// <summary>
        /// Siparişleri listeler Admin için
        /// </summary>
        /// <param name="UserId"></param>
        /// <returns></returns>
        public async Task<IActionResult> Index()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {
                FirebaseResponse response = await dbcontext.client.GetAsync("Orders");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Order>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Order>(((JProperty)item).Value.ToString()));
                    }
                    return View(list);
                }
            }
            return RedirectToAction("SignIn", "Account");
        }

        public async Task<IActionResult> Siparislerim()
        {
            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId"); 

            if (token != null)
            {
                FirebaseResponse response = await dbcontext.client.GetAsync("Orders");
                dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
                var list = new List<Order>();
                if (data != null)
                {
                    foreach (var item in data)
                    {
                        list.Add(JsonConvert.DeserializeObject<Order>(((JProperty)item).Value.ToString()));
                    }
                    return View(list.Where(x=> x.UserId == userid));
                }
            }
            return RedirectToAction("SignIn", "Account");
        }
          
        /// <summary>
        /// Siparişi onaylar ürün miktarı düşer admin için
        /// </summary>
        /// <param name="ProductId"></param>
        /// <param name="UserId"></param>
        /// <returns></returns>

        public async Task<IActionResult> OrderOK(string orderId)
        {

            var token = HttpContext.Session.GetString("_UserToken");
            var userid = HttpContext.Session.GetString("_UserId");
            var userRole = HttpContext.Session.GetString("_UserRole");

            if (token != null && userRole == "admin")
            {
                //Siparişi bul
                FirebaseResponse response = dbcontext.client.Get("Orders/" + orderId);
                Order data = JsonConvert.DeserializeObject<Order>(response.Body);
                data.IsOk = true;
                // ORder tablosunda IsOk tru yap yani siparişi onayla
                FirebaseResponse setResponseorder = await dbcontext.client.UpdateAsync("Product/" + data.Id, data);

                //siparişteki ürünleri bul
                FirebaseResponse responseSepet = dbcontext.client.Get("Orderline/");
                dynamic orderline = JsonConvert.DeserializeObject<dynamic>(responseSepet.Body);
                var list = new List<Orderline>();
                if (orderline != null)
                {
                    foreach (var item in orderline)
                    {
                        list.Add(JsonConvert.DeserializeObject<Orderline>(((JProperty)item).Value.ToString()));
                    }
                }
                var orderlines = list.Where(x => x.OrderId == data.Id).ToList();
                if (orderlines != null)
                {
                    foreach (var item in orderlines)
                    {
                        //ürünü çağır
                        FirebaseResponse responseproduct = dbcontext.client.Get("Product/" + item.ProductId);
                        Product dataproduct = JsonConvert.DeserializeObject<Product>(response.Body);
                        //Stoktaki üründen siparişteki miktar çıktığında 0dan fazla kalıyorsa. stok miktarını siparişteki miktar kadar azalt
                        if ((dataproduct.Stock - item.Quantity) > 0)
                        {
                            // siparişteki miktar kadar azalt
                            dataproduct.Stock -= item.Quantity;
                            FirebaseResponse setResponseproduct = await dbcontext.client.UpdateAsync("Product/" + item.ProductId, dataproduct);
                            Product setproduct = setResponseproduct.ResultAs<Product>();
                        }
                        else
                        {
                            //siparişten sonra ürün kalmayacaksa ürünü sil
                            FirebaseResponse responseproductDelete = await dbcontext.client.DeleteAsync("Product/" + item.ProductId);
                        }
                    }
                }
                return RedirectToAction("Index", "Order");
            }
            return RedirectToAction("SignIn", "Account");
        }
    }
}
 