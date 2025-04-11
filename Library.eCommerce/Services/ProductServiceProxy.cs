using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring2025_andresg.Models;

namespace Library.eCommerce.Services
{
    public class ProductServiceProxy
    {
        private ProductServiceProxy()
        {
            Products = new List<Product?>();
        }

        private int LastKey
        {
            get
            {
                if (!Products.Any())
                {
                    return 0;
                }

                return Products.Select(p => p?.Id ?? 0).Max();
            }

        }

        private static ProductServiceProxy? instance;
        private static object instanceLock = new object();
        public static ProductServiceProxy Current
        {
            get
            {
                lock (instanceLock)
                {
                    if (instance == null)
                    {
                        instance = new ProductServiceProxy();
                    }
                    return instance;
                }

            }
        }

        public List<Product?> Products { get; private set; }

        public Product AddOrUpdate(Product product)
        {
            if (product.Id == 0)
            {
                product.Id = LastKey + 1;
                Products.Add(product);
            }
            else
            {
                var existing = Products.FirstOrDefault(p => p?.Id == product.Id);
                if (existing != null)
                {
                    existing.Name = product.Name;
                    existing.Price = product.Price;
                    existing.Quantity = product.Quantity;
                }

            }

            return product;
        }

        public Product? Delete(int Id)
        {
            if (Id == 0)
            {
                return null;
            }

            Product? product = Products.FirstOrDefault(p => p?.Id == Id);
            Products.Remove(product);

            return product;
        }

        // methods for shopping cart

        public void ManageShoppingCart()
        {
            bool back = false;
            while (!back)
            {
                Console.WriteLine();
                Console.WriteLine("Shopping Cart Menu:");
                Console.WriteLine(" A. Add product to cart");
                Console.WriteLine(" V. View cart items");
                Console.WriteLine(" U. Update cart item quantity");
                Console.WriteLine(" R. Remove product from cart");
                Console.WriteLine(" B. Back to Main Menu");
                Console.Write("Enter option: ");

                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                    continue;
                char choice = input[0];

                switch (choice)
                {
                    case 'A':
                    case 'a':
                        AddProductToCart();
                        break;
                    case 'V':
                    case 'v':
                        ViewCartItems();
                        break;
                    case 'U':
                    case 'u':
                        UpdateCartItem();
                        break;
                    case 'R':
                    case 'r':
                        RemoveCartItem();
                        break;
                    case 'B':
                    case 'b':
                        back = true;
                        break;
                    default:
                        Console.WriteLine("Invalid choice.");
                        break;
                }
            }
        }

        private void AddProductToCart()
        {
            // List inventory items.
            if (!Products.Any())
            {
                Console.WriteLine("No inventory items available.");
                return;
            }
            Console.WriteLine("Available Inventory:");
            foreach (var product in Products)
            {
                Console.WriteLine(product);
            }
            Console.Write("Enter the product ID to add: ");
            int id = int.Parse(Console.ReadLine() ?? "-1");
            var productItem = Products.FirstOrDefault(p => p?.Id == id);
            if (productItem == null)
            {
                Console.WriteLine("Product not found.");
                return;
            }
            Console.Write("Enter quantity to add: ");
            int qty = int.Parse(Console.ReadLine() ?? "0");
            if (qty > productItem.Quantity)
            {
                Console.WriteLine($"Insufficient stock. Only {productItem.Quantity} available.");
                return;
            }
            // Deduct from inventory.
            productItem.Quantity -= qty;
            AddOrUpdate(productItem);
            // Add to cart.
            ShoppingCart.Current.AddItem(productItem, qty);
            Console.WriteLine("Product added to cart.");
        }

        private void ViewCartItems()
        {
            if (!ShoppingCart.Current.Items.Any())
            {
                Console.WriteLine("Shopping cart is empty.");
                return;
            }
            Console.WriteLine("Items in Cart:");
            foreach (var item in ShoppingCart.Current.Items)
            {
                Console.WriteLine(item);
            }
        }

        private void UpdateCartItem()
        {
            Console.Write("Enter the product ID in the cart to update: ");
            int id = int.Parse(Console.ReadLine() ?? "-1");
            var cartItem = ShoppingCart.Current.Items.FirstOrDefault(ci => ci.Product.Id == id);
            if (cartItem == null)
            {
                Console.WriteLine("Product not found in cart.");
                return;
            }
            // Restore current cart quantity to inventory.
            var inventoryItem = Products.FirstOrDefault(p => p?.Id == id);
            if (inventoryItem != null)
            {
                inventoryItem.Quantity += cartItem.Quantity;
                AddOrUpdate(inventoryItem);
            }
            Console.Write("Enter new desired quantity: ");
            int newQty = int.Parse(Console.ReadLine() ?? "0");
            if (inventoryItem == null || newQty > inventoryItem.Quantity)
            {
                Console.WriteLine($"Insufficient stock. Only {inventoryItem?.Quantity} available.");
                // Revert restoration: deduct the cart quantity again.
                if (inventoryItem != null)
                {
                    inventoryItem.Quantity -= cartItem.Quantity;
                    AddOrUpdate(inventoryItem);
                }
                return;
            }
            int diff = newQty - cartItem.Quantity;
            if (diff > 0)
            {
                inventoryItem.Quantity -= diff;
            }
            else if (diff < 0)
            {
                inventoryItem.Quantity += -diff;
            }
            AddOrUpdate(inventoryItem);
            ShoppingCart.Current.UpdateItemQuantity(id, newQty);
            Console.WriteLine("Cart item updated.");
        }

        private void RemoveCartItem()
        {
            Console.Write("Enter the product ID to remove from cart: ");
            int id = int.Parse(Console.ReadLine() ?? "-1");
            var cartItem = ShoppingCart.Current.Items.FirstOrDefault(ci => ci.Product.Id == id);
            if (cartItem == null)
            {
                Console.WriteLine("Product not found in cart.");
                return;
            }
            // Return quantity to inventory.
            var inventoryItem = Products.FirstOrDefault(p => p?.Id == id);
            if (inventoryItem != null)
            {
                inventoryItem.Quantity += cartItem.Quantity;
                AddOrUpdate(inventoryItem);
            }
            ShoppingCart.Current.RemoveItem(id);
            Console.WriteLine("Product removed from cart.");
        }

        public void Checkout()
        {
            Console.WriteLine();
            Console.WriteLine("----- Checkout -----");
            if (!ShoppingCart.Current.Items.Any())
            {
                Console.WriteLine("Your cart is empty.");
            }
            else
            {
                decimal subtotal = ShoppingCart.Current.CalculateSubtotal();
                foreach (var item in ShoppingCart.Current.Items)
                {
                    Console.WriteLine(item);
                }
                Console.WriteLine("--------------------");
                Console.WriteLine($"Subtotal: {subtotal:C}");
                decimal tax = subtotal * 0.07m;
                Console.WriteLine($"Sales Tax (7%): {tax:C}");
                Console.WriteLine($"Total: {(subtotal + tax):C}");
                Console.WriteLine("Thank you for your purchase!");
            }
        }
    }
}
