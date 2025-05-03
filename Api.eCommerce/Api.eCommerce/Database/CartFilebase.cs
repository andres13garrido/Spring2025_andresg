using Library.eCommerce.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Api.eCommerce.Database
{
    public class CartFilebase
    {
        private readonly string _cartRoot;
        private static CartFilebase _instance;
        public static CartFilebase Current => _instance ??= new CartFilebase();

        private CartFilebase()
        {
            var root = @"C:\temp";
            _cartRoot = Path.Combine(root, "Cart");
            if (!Directory.Exists(_cartRoot))
                Directory.CreateDirectory(_cartRoot);
        }

        public List<Item> CartItems
        {
            get
            {
                return Directory
                    .GetFiles(_cartRoot, "*.json")
                    .Select(f => JsonConvert.DeserializeObject<Item>(File.ReadAllText(f)))
                    .Where(i => i != null)
                    .ToList()!;
            }
        }

        public Item Add(int inventoryId)
        {
            // 1) Pull the inventory item
            var inv = Filebase.Current.Inventory.FirstOrDefault(i => i!.Id == inventoryId);
            if (inv == null || inv.Quantity <= 0) return null!;

            // 2) Decrement inventory and persist it
            inv.Quantity--;
            Filebase.Current.AddOrUpdate(inv);

            // 3) Add or update in cart
            var existing = CartItems.FirstOrDefault(i => i.Id == inventoryId);
            if (existing == null)
            {
                var newItem = new Item(inv) { Quantity = 1 };
                PersistCartItem(newItem);
                return newItem;
            }
            else
            {
                existing.Quantity = (existing.Quantity ?? 0) + 1;
                PersistCartItem(existing);
                return existing;
            }
        }

        public Item Remove(int cartItemId)
        {
            var cartItem = CartItems.FirstOrDefault(i => i.Id == cartItemId);
            if (cartItem == null) return null!;

            // Decrement or delete in cart
            cartItem.Quantity--;
            var path = Path.Combine(_cartRoot, $"{cartItem.Id}.json");
            if (cartItem.Quantity <= 0)
                File.Delete(path);
            else
                File.WriteAllText(path, JsonConvert.SerializeObject(cartItem));

            // Return to inventory
            var inv = Filebase.Current.Inventory.FirstOrDefault(i => i!.Id == cartItemId)!;
            inv.Quantity++;
            Filebase.Current.AddOrUpdate(inv);

            return cartItem;
        }

        public void ClearCart()
        {
            foreach (var f in Directory.GetFiles(_cartRoot, "*.json"))
                File.Delete(f);
        }

        public Receipt Checkout()
        {
            var items = CartItems;
            var total = items
                .Sum(i => (i.Product.Price) * (i.Quantity ?? 0));

            ClearCart();

            return new Receipt
            {
                Items = items,
                Total = total,
                Timestamp = DateTime.UtcNow
            };
        }


        public IEnumerable<Item> Search(string? q)
        {
            return CartItems
                   .Where(i => i.Product?.Name
                       .Contains(q ?? string.Empty, StringComparison.OrdinalIgnoreCase) ?? false);
        }

        private void PersistCartItem(Item item)
        {
            var path = Path.Combine(_cartRoot, $"{item.Id}.json");
            File.WriteAllText(path, JsonConvert.SerializeObject(item));
        }
    }
}
