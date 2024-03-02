namespace CarAuctionManagementSystem.Domain
{
    using CarAuctionManagementSystem.Models;

    public class Auction(IVehicle vehicle, decimal startingBid)
    {
        public decimal CurrentHighestBid { get; private set; } = startingBid;

        public string CurrentHighestBidder { get; private set; } = "Start Bid";

        public bool IsActive { get; private set; } = false;

        public IVehicle AssociatedVehicle { get; } = vehicle;

        public void Start()
        {
            this.IsActive = true;
        }

        public void Close()
        {
            this.IsActive = false;
        }

        public void PlaceBid(decimal amount, string bidderName)
        {
            if (!this.IsActive)
            {
                throw new InvalidOperationException("Auction is not active.");
            }

            if (amount <= this.CurrentHighestBid)
            {
                throw new InvalidOperationException("Bid amount must be higher than the current highest bid.");
            }

            this.CurrentHighestBid = amount;
            this.CurrentHighestBidder = bidderName;
        }
    }
}
