using E_Commerce.MvcWebUI.Entity; 
using Microsoft.AspNetCore.Mvc; 
using Firebase.Database;
using Firebase.Database.Query;
using System.Text.Json;
using System.Text.Json.Serialization;
namespace E_Commerce.MvcWebUI.Components
{
    public class CpIndexCategoriesList : ViewComponent
    {
        DbContext dbcontext;

        public CpIndexCategoriesList()
        {
            dbcontext = new DbContext();
        }
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var categories = await dbcontext.firebaseClient
               .Child("Category")
               .OnceAsync<Category>();
            return View(categories.ToList());
        }
    }
}
