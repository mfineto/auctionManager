namespace CarAuctionManagementSystem.Models
{
    public class SUV(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, int seats) : IVehicle
    {
        public string UniqueIdentifier { get; } = uniqueIdentifier;

        public string Manufacturer { get; } = manufacturer;

        public string Model { get; } = model;

        public int Year { get; } = year;

        public decimal StartingBid { get; } = startingBid;

        public int NumberOfSeats { get; } = seats;

        public void DisplayDetails()
        {
            Console.WriteLine($"Manufacturer: {this.Manufacturer}");
            Console.WriteLine($"Model: {this.Model}");
            Console.WriteLine($"Year: {this.Year}");
            Console.WriteLine($"Starting Bid: {this.StartingBid}");
            Console.WriteLine($"Number of Seats: {this.NumberOfSeats}");
        }
    }
}
