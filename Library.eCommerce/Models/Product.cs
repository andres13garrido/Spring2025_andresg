using System;
using System.Collections.Generic;
using System.Dynamic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Spring2025_andresg.Models
{
    public class Product
    {

        public int Id { get; set; } // auto property

        public string? Name { get; set; } // auto property

        public string? Display
        {
            get
            {
                return $"{Id}. {Name} ";
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
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }

    }
}
