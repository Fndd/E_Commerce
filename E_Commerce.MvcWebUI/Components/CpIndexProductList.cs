using E_Commerce.MvcWebUI.Entity; 
using Microsoft.AspNetCore.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace E_Commerce.MvcWebUI.Components
{
    public class CpIndexProductList : ViewComponent
    {
        DbContext dbcontext;

        public CpIndexProductList()
        {
            dbcontext = new DbContext();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            FirebaseResponse response = dbcontext.client.Get("Product");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Product>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Product>(((JProperty)item).Value.ToString()));
            }
            return View(list.Take(6).ToList()); 
        }
    }
}
