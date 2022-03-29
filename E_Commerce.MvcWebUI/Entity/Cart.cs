namespace E_Commerce.MvcWebUI.Entity
{
    public class Cart
    {
        public string Id { get; set; } = String.Empty;
        public int ProductId { get; set; }
        public int Quantity { get; set; }
        public double RowTotal { get; set; }
    }
}
