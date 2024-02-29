using System;

namespace CarAuctionManagementSystem.Models
{
    public class Truck : IVehicle
    {
        public string UniqueIdentifier { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }
        public int LoadCapacity { get; }

        public Truck(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, int loadCapacity)
        {
            UniqueIdentifier = uniqueIdentifier;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            LoadCapacity = loadCapacity;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Starting Bid: {StartingBid}");
            Console.WriteLine($"Load Capacity: {LoadCapacity}");
        }
    }
}
