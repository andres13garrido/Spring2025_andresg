using System;
using Library.eCommerce.Services;
using Spring2025_andresg.Models;

namespace Spring2025_andresg
{
    internal class Program
    {
        
        static void Main(string[] args)
        {

            Console.WriteLine("Welcome to My Store!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory item");
            Console.WriteLine("U. Update inventory item");
            Console.WriteLine("D. Delete inventory item");
            Console.WriteLine();
           


            List<Product?> list = ProductServiceProxy.Current.Products;


            char choice = '\0';
            do
            {
                Console.WriteLine("Enter option:");
                string? input = Console.ReadLine();
                if (string.IsNullOrEmpty(input))
                {
                    Console.WriteLine("Invalid input. Please try again.");
                    continue;
                }
                choice = input[0];

                switch (choice)
                {
                    case 'C':
                    case 'c':

                        // create a new product
                        Console.WriteLine("Enter product name:");
                        string name = Console.ReadLine() ?? "ERROR";
                        Console.WriteLine("Enter product price:");
                        decimal price = decimal.Parse(Console.ReadLine() ?? "0");
                        Console.WriteLine("Enter product quantity:");
                        int quantity = int.Parse(Console.ReadLine() ?? "0");
                        ProductServiceProxy.Current.AddOrUpdate(new Product()
                        {
                            Name = name,
                            Price = price,
                            Quantity = quantity
                        });
                        break;
                    case 'R':
                    case 'r':
                        list.ForEach(Console.WriteLine);
                        break;
                    case 'U':
                    case 'u':
                        // select one product and replace it with a new product
                        Console.WriteLine("Enter which product you would like to update:");
                        int selection = int.Parse(Console.ReadLine() ?? "-1");
                        var selectedProduct = list.FirstOrDefault(p => p?.Id == selection);

                        if (selectedProduct != null)
                        {
                            selectedProduct.Name = Console.ReadLine() ?? "ERROR";
                            ProductServiceProxy.Current.AddOrUpdate(selectedProduct);
                        }

                        break;
                    case 'D':
                    case 'd':
                        // select one product and remove it from the list
                        Console.WriteLine("Enter which product you would like to update:");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        ProductServiceProxy.Current.Delete(selection);
                        break;
                   
                }

            } while (choice != 'Q' && choice != 'q');

            Console.ReadLine();
        }
    }
}


//vid1 done
//vid2 done
//vid3 done
//vid4 done
//vid5 done
//vid6 done
//vid 7 done
//vid 8 done
//vid9 done
//vid 10 done
//vid 11 done
//vid 12 done 24
//vid 13 start 26 VERY IMPORTANT SHALLOW/DEEP COPY
