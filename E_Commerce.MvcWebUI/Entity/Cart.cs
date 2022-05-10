namespace E_Commerce.MvcWebUI.Entity
{
    public class Cart
    {
        public string Id { get; set; } = String.Empty;
        public string ProductId { get; set; }
        public int Quantity { get; set; }
        public double RowTotal { get; set; } 
        public double Price { get; set; }
        public double Discount { get; set; }
        public double DiscountedPrice { get; set; }
        public string Name { get; set; } = String.Empty; 
        public string Image { get; set; } = String.Empty;
        public string ProductCode { get; set; } = String.Empty;  
    }
}
