using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Library.eCommerce.DTO;

namespace Spring2025_andresg.Models
{
    public class Product
    {

        public int Id { get; set; } // auto property

        public string? Name { get; set; } // auto property

        public decimal Price { get; set; } // auto property

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} - {Price}";
            }
        }


        public Product()
        {
            Name = string.Empty;
        }
        public Product(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }

        public Product(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }
    }
}
