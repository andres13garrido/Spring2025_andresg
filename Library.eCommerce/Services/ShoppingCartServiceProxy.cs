using Library.eCommerce.Models;
using Library.eCommerce.Utilities;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Library.eCommerce.Services
{
    public class ShoppingCartServiceProxy
    {
        private static ShoppingCartServiceProxy? _instance;
        private static readonly object _lock = new object();
        private readonly WebRequestHandler _http = new WebRequestHandler();

        /// <summary>
        /// In‐memory cache of the last GET /ShoppingCart result.
        /// </summary>
        public List<Item> CartItems { get; private set; } = new();

        private ShoppingCartServiceProxy() { }

        public static ShoppingCartServiceProxy Current
        {
            get
            {
                lock (_lock)
                {
                    return _instance ??= new ShoppingCartServiceProxy();
                }
            }
        }

        /// <summary>
        /// Reloads CartItems by calling GET /ShoppingCart.
        /// </summary>
        public async Task Refresh()
        {
            var json = await _http.Get("/ShoppingCart");
            CartItems = JsonConvert
                .DeserializeObject<List<Item>>(json)
                ?? new List<Item>();
        }

        /// <summary>
        /// POST /ShoppingCart/Search { query }  
        /// Filters cart on the server.
        /// </summary>
        public async Task<IEnumerable<Item>> Search(string? query)
        {
            var json = await _http.Post("/ShoppingCart/Search", new QueryRequest { Query = query ?? string.Empty });
            CartItems = JsonConvert
                .DeserializeObject<List<Item>>(json)
                ?? new List<Item>();
            return CartItems;
        }

        /// <summary>
        /// POST /ShoppingCart/Purchase/{id}  
        /// Moves one unit from inventory into the cart.
        /// </summary>
        public async Task<Item?> Purchase(int id)
        {
            var json = await _http.Post($"/ShoppingCart/Purchase/{id}", new object());
            var item = JsonConvert.DeserializeObject<Item>(json);
            if (item == null) return null;

            var existing = CartItems.FirstOrDefault(i => i.Id == id);
            if (existing == null)
                CartItems.Add(item);
            else
                existing.Quantity = item.Quantity;

            return item;
        }

        /// <summary>
        /// POST /ShoppingCart/Return/{id}  
        /// Moves one unit from cart back to inventory.
        /// </summary>
        public async Task<Item?> Return(int id)
        {
            var json = await _http.Post($"/ShoppingCart/Return/{id}", new object());
            var item = JsonConvert.DeserializeObject<Item>(json);
            if (item == null) return null;

            if (item.Quantity <= 0)
                CartItems.RemoveAll(i => i.Id == id);
            else
            {
                var existing = CartItems.First(i => i.Id == id);
                existing.Quantity = item.Quantity;
            }

            return item;
        }

        /// <summary>
        /// POST /ShoppingCart/Checkout  
        /// Empties the cart and returns a Receipt.
        /// </summary>
        public async Task<Receipt?> Checkout()
        {
            var json = await _http.Post("/ShoppingCart/Checkout", new object());
            var receipt = JsonConvert.DeserializeObject<Receipt>(json);
            CartItems.Clear();
            return receipt;
        }
    }
}
