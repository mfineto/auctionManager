using System;

namespace CarAuctionManagementSystem.Models
{
    public class SUV : IVehicle
    {
        public string UniqueIdentifier { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }
        public int NumberOfSeats { get; }

        public SUV(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, int seats)
        {
            UniqueIdentifier = uniqueIdentifier;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            NumberOfSeats = seats;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Starting Bid: {StartingBid}");
            Console.WriteLine($"Number of Seats: {NumberOfSeats}");
        }
    }
}
