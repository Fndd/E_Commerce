namespace E_Commerce.MvcWebUI.Entity
{
    public class Cart
    {
        public string Id { get; set; } = String.Empty;
        public string ProductId { get; set; }
        public virtual Product Product { get; set; }
        public int Quantity { get; set; }
        public double RowTotal { get; set; }
    }
}
