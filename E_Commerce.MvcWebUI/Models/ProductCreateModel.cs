using E_Commerce.MvcWebUI.Entity;

namespace E_Commerce.MvcWebUI.Models
{
    public class ProductCreateModel
    {
        public Product?  Product { get; set; }
        public List<Category> Categories { get; set; }
    }
}
