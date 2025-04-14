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

        public decimal Price { get; set; } // auto property

        public int Quantity { get; set; } // auto property
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
            Price = 0;
            Quantity = 0;
        }

        public override string ToString()
        {
            return Display ?? string.Empty;
        }

    }
}
