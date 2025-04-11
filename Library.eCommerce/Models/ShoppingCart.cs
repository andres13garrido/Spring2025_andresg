using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_andresg.Models
{
    // Represents a product entry in the shopping cart.
    public class CartItem
    {
        public Product Product { get; set; }
        public int Quantity { get; set; }

        public CartItem(Product product, int quantity)
        {
            Product = product;
            Quantity = quantity;
        }

        public override string ToString()
        {
            return $"{Product.Name} - Price: {Product.Price:C} x {Quantity}";
        }
    }

    public class ShoppingCart
    {
        // A singleton instance for simplicity.
        private static ShoppingCart? instance;
        private static object instanceLock = new object();

        public static ShoppingCart Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                        instance = new ShoppingCart();
                    return instance;
                }
            }
        }

        public List<CartItem> Items { get; private set; } = new List<CartItem>();

        public void AddItem(Product product, int quantity)
        {
            // Check if product is already in cart.
            var existing = Items.FirstOrDefault(ci => ci.Product.Id == product.Id);
            if (existing != null)
            {
                existing.Quantity += quantity;
            }
            else
            {
                Items.Add(new CartItem(product, quantity));
            }
        }

        public void UpdateItemQuantity(int productId, int newQuantity)
        {
            var item = Items.FirstOrDefault(ci => ci.Product.Id == productId);
            if (item != null)
            {
                item.Quantity = newQuantity;
            }
        }

        public void RemoveItem(int productId)
        {
            var item = Items.FirstOrDefault(ci => ci.Product.Id == productId);
            if (item != null)
            {
                Items.Remove(item);
            }
        }

        public decimal CalculateSubtotal()
        {
            return Items.Sum(ci => ci.Product.Price * ci.Quantity);
        }
    }
}