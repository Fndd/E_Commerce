using E_Commerce.MvcWebUI.Entity; 
using Microsoft.AspNetCore.Mvc;
using FireSharp.Config;
using FireSharp.Interfaces;
using FireSharp.Response;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace E_Commerce.MvcWebUI.Components
{
    public class CpLeftCategoriesList : ViewComponent
    {
        DbContext dbcontext;

        public CpLeftCategoriesList()
        {
            dbcontext = new DbContext();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            FirebaseResponse response = dbcontext.client.Get("Category");
            dynamic data = JsonConvert.DeserializeObject<dynamic>(response.Body);
            var list = new List<Category>();
            foreach (var item in data)
            {
                list.Add(JsonConvert.DeserializeObject<Category>(((JProperty)item).Value.ToString()));
            }
            return View(list);
        }
    }
}
