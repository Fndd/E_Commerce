﻿namespace E_Commerce.MvcWebUI.Entity
{
    public class Order
    {
        public string Id { get; set; } = String.Empty;
        public string OrderNumber { get; set; } = String.Empty;
        public double Total { get; set; }
        public string UserId { get; set; } = String.Empty;
    }
}
