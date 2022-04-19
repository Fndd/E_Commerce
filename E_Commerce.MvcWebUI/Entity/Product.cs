namespace E_Commerce.MvcWebUI.Entity
{
    public class Product
    {
        public string Id { get; set; } = String.Empty;
        public string CategoryId { get; set; } = String.Empty;
        public int Stock { get; set; } 
        public int Like { get; set; }
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public string Name { get; set; } = String.Empty;
        public string Description { get; set; } = String.Empty;
        public string Image { get; set; } = String.Empty;
        public string ProductCode { get; set; } = String.Empty;
        public bool IsHome { get; set; }
        public bool IsApproved { get; set; }
        public bool OnSale { get; set; }

    }
}
