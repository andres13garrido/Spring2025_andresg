using System;
using Library.eCommerce.Services;
using Spring2025_andresg.Models;

namespace Spring2025_andresg
{
    internal class Program
    {
        static void Main(string[] args)
        {

            var lastKey = 1;
            Console.WriteLine("Welcome to My Store!");

            Console.WriteLine("C. Create new inventory item");
            Console.WriteLine("R. Read all inventory item");
            Console.WriteLine("U. Update inventory item");
            Console.WriteLine("D. Delete inventory item");
            Console.WriteLine("Q. Quit");


            List<Product?> list = ProductServiceProxy.Current.Products;


            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];

                switch (choice)
                {
                    case 'C':
                    case 'c':
                       ProductServiceProxy.Current.Products.Add(new Product()
                       {
                           
                           Name = Console.ReadLine()
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
                        var selectedProduct = list.FirstOrDefault(p => p.Id == selection);
                        if (selectedProduct != null)
                        {
                            selectedProduct.Name = Console.ReadLine() ?? "ERROR";

                        } 

                        break;
                    case 'D':
                    case 'd':
                        // select one product and remove it from the list
                        Console.WriteLine("Enter which product you would like to update:");
                        selection = int.Parse(Console.ReadLine() ?? "-1");
                        selectedProduct = list.FirstOrDefault(p => p.Id == selection);
                        list.Remove(selectedProduct);
                        break;
                    case 'Q':
                    case 'q':
                        break;
                    default:
                        Console.WriteLine("Invalid choice");
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
//vid4 0:00
