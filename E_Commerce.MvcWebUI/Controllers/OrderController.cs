using E_Commerce.MvcWebUI.Entity;
using Microsoft.AspNetCore.Mvc; 
using System.Text.Json;
using System.Text.Json.Serialization;

namespace E_Commerce.MvcWebUI.Controllers
{
    public class OrderController : Controller
    {
        //    DbContext dbcontext;

        //    public OrderController()
        //    {
        //        dbcontext = new DbContext();
        //    }
        //    public async Task<IActionResult> Index()
        //    {
        //        var order = await dbcontext.firebaseClient
        //          .Child("Order")
        //          .OnceAsync<Order>();
        //        return View(order.ToList());
        //    }
        //    public async Task<IActionResult> OrderDetail(string id)
        //    {
        //        var order = await dbcontext.firebaseClient
        //          .Child("Order")
        //          .OnceAsync<Order>();
        //        return View(order.ToList().Where(x=> x.Object.Id == id).FirstOrDefault());
        //    }
        //    public IActionResult Add()
        //    {
        //        return View();
        //    }
        //    [HttpPost]
        //    public async Task<IActionResult> Add(Order order)
        //    {
        //        string guid = Guid.NewGuid().ToString();
        //        order.Id = guid;
        //        string jsonString = JsonSerializer.Serialize(order);
        //        var result = await dbcontext.firebaseClient
        //       .Child("Order")
        //       .PostAsync(jsonString);
        //        return View();
        //    }
        //    [HttpPost]
        //    public async Task<IActionResult> AddOrderline(Orderline orderline)
        //    {
        //        string guid = Guid.NewGuid().ToString();
        //        orderline.Id = guid;
        //        string jsonString = JsonSerializer.Serialize(orderline);
        //        var result = await dbcontext.firebaseClient
        //       .Child("Orderline")
        //       .PostAsync(jsonString);
        //        return View();
        //    }

        //    public async Task<IActionResult> Edit(string id)
        //    {
        //        var order = await dbcontext.firebaseClient
        //           .Child("Order")
        //           .Child(id)
        //           .OnceAsync<Order>();
        //        return View(order.FirstOrDefault());
        //    }

        //    [HttpPost]
        //    public async Task<IActionResult> Edit(Order order)
        //    {
        //        string jsonString = JsonSerializer.Serialize(order);
        //        await dbcontext.firebaseClient
        //       .Child("Order")
        //       .Child(order.Id)
        //       .PutAsync(jsonString);
        //        return View();
        //    }
        //    public async Task<IActionResult> Delete(string id)
        //    {
        //        await dbcontext.firebaseClient
        //        .Child("Order")
        //        .Child(id)
        //        .DeleteAsync();
        //        return View();
        //    }
        //    public async Task<IActionResult> DeleteOrderline(string id)
        //    {
        //        await dbcontext.firebaseClient
        //        .Child("Orderline")
        //        .Child(id)
        //        .DeleteAsync();
        //        return View();
        //    }
        //}
    }
}