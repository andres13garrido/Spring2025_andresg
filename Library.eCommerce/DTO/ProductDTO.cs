using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Spring2025_andresg.Models;

namespace Library.eCommerce.DTO
{
    public class ProductDTO
    {
        public int Id { get; set; } // auto property

        public string? Name { get; set; } // auto property

        public decimal Price { get; set; } // auto property

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} ";
            }
        }

        public ProductDTO()
        {
            Name = string.Empty;
        }
        public ProductDTO(Product p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }
        public ProductDTO(ProductDTO p)
        {
            Name = p.Name;
            Id = p.Id;
            Price = p.Price;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }
    }
}
