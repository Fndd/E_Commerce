namespace E_Commerce.MvcWebUI.Entity
{
    public class Orderline
    {
        public string Id { get; set; } = String.Empty;
        public string OrderId { get; set; } = String.Empty;
        public string ProductId { get; set; } = String.Empty;
        public int Quantity { get; set; }
        public double Price { get; set; }
        public double RowTotal { get; set; }
    }
}
