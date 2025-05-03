using Api.eCommerce.Database;
using Library.eCommerce.Models;
using System.Collections.Generic;

namespace Api.eCommerce.EC
{
    public class ShoppingCartEC
    {
        public IEnumerable<Item> GetAll() =>
            CartFilebase.Current.CartItems;

        public Item Purchase(int id) =>
            CartFilebase.Current.Add(id);

        public Item Return(int id) =>
            CartFilebase.Current.Remove(id);

        public Receipt FinalizePurchase() =>
            CartFilebase.Current.Checkout();

        public IEnumerable<Item> Search(string query) =>
            CartFilebase.Current.Search(query);
    }
}
