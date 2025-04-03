using System;

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
            Console.WriteLine("Q. Quit");


            List<string> list = new List<string>();


            char choice;
            do
            {
                string? input = Console.ReadLine();
                choice = input[0];

                switch (choice)
                {
                    case 'C':
                    case 'c':
                        var newProduct = Console.ReadLine() ?? "UNKNOWN";

                        // add to a list
                        list.Add(newProduct);
                        break;
                    case 'R':
                    case 'r':
                        // print out all the products in our list
                        break;
                    case 'U':
                    case 'u':
                        // select one product and replace it with a new product
                        break;
                    case 'D':
                    case 'd':
                        // select one product and remove it from the list
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
//vid2 minute: 