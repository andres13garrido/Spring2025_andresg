using Api.eCommerce.Database;
using Library.eCommerce.Models;
using Microsoft.AspNetCore.Mvc;

namespace Api.eCommerce.EC
{
    public class InventoryEC
    {

        public List<Item?> Get()
        {
            return Filebase.Current.Inventory;
        }

        public IEnumerable<Item> Get(string? query)
        {
            return FakeDatabase.Search(query).Take(100) ?? new List<Item>();
        }

        //public Item? Delete(int id)
        //{
        //    var itemToDelete = Filebase.Current.Inventory.FirstOrDefault(i => i?.Id == id);
        //    if (itemToDelete != null)
        //    {
        //        //Filebase.Current.Delete(itemToDelete);
        //    }

        //    return itemToDelete;
        //}

        public Item? Delete(int id)
        {
            // find the item in our in-memory list
            var itemToDelete = Filebase.Current.Inventory.FirstOrDefault(i => i?.Id == id);
            if (itemToDelete != null)
            {
                // delete the JSON file on disk
                // (assuming your Delete method is on Filebase.Current and takes type + id)
                Filebase.Current.Delete("product", id.ToString());

                // remove it from the in-memory list
                Filebase.Current.Inventory.Remove(itemToDelete);
            }

            // return the deleted item (or null if not found)
            return itemToDelete;
        }


        public Item? AddOrUpdate(Item item)
        {
            //if (item.Id == 0)
            //{
            //    item.Id = Filebase.Current.LastKey + 1;
            //    item.Product.Id = item.Id;
            //    Filebase.Current.Inventory.Add(item);
            //} else {
            //    var existingItem = Filebase.Current.Inventory.FirstOrDefault(p => p.Id == item.Id);
            //    var index = Filebase.Current.Inventory.IndexOf(existingItem);
            //    Filebase.Current.Inventory.RemoveAt(index);
            //    Filebase.Current.Inventory.Insert(index, new Item(item));
            //}

            return Filebase.Current.AddOrUpdate(item);
        }


    }

}
