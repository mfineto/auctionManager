using System;
using CarAuctionManagementSystem.Controllers;

namespace CarAuctionManagementSystem
{
    public class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Welcome to Car Auction Management System!");

            // Instantiate AuctionController and run your application logic
            var auctionController = new AuctionController();
            auctionController.Run();

            Console.WriteLine("Press any key to exit...");
            Console.ReadKey();
        }
    }
}
