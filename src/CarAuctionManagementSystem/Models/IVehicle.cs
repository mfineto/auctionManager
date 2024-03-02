namespace CarAuctionManagementSystem.Models
{
    public interface IVehicle
    {
        string UniqueIdentifier { get; }

        string Manufacturer { get; }

        string Model { get; }

        int Year { get; }

        decimal StartingBid { get; }

        void DisplayDetails();
    }
}