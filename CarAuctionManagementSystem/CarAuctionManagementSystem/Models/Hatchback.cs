namespace CarAuctionManagementSystem.Models
{
    public class Hatchback : IVehicle
    {
        public string UniqueIdentifier { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }
        public int NumberOfDoors { get; }

        public Hatchback(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, int doors)
        {
            UniqueIdentifier = uniqueIdentifier;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
            NumberOfDoors = doors;
        }

        public void DisplayDetails()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Starting Bid: {StartingBid}");
            Console.WriteLine($"Number of Doors: {NumberOfDoors}");
        }
    }
}
