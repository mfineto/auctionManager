namespace CarAuctionManagementSystem.Models
{
    public abstract class Vehicle
    {
        public string UniqueIdentifier { get; }
        public string Manufacturer { get; }
        public string Model { get; }
        public int Year { get; }
        public decimal StartingBid { get; }

        public Vehicle(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, Dictionary<string, object> additionalParameters = null)
        {
            UniqueIdentifier = uniqueIdentifier;
            Manufacturer = manufacturer;
            Model = model;
            Year = year;
            StartingBid = startingBid;
        }

        public virtual void DisplayDetails()
        {
            Console.WriteLine($"Manufacturer: {Manufacturer}");
            Console.WriteLine($"Model: {Model}");
            Console.WriteLine($"Year: {Year}");
            Console.WriteLine($"Starting Bid: {StartingBid:C}");
        }
    }
}
