// Library.eCommerce.Models/Receipt.cs
using System;
using System.Collections.Generic;

namespace Library.eCommerce.Models
{
    public class Receipt
    {
        public List<Item> Items { get; set; } = new List<Item>();
        public decimal Total { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
