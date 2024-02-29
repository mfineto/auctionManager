using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Domain
{
    public class Auction
    {
        public decimal CurrentHighestBid { get; private set; }
        public string CurrentHighestBidder { get; private set; }
        public bool IsActive { get; private set; }
        public IVehicle AssociatedVehicle { get; }


        public Auction(IVehicle vehicle, decimal startingBid)
        {
            AssociatedVehicle = vehicle;
            IsActive = false;
            CurrentHighestBid = startingBid;
            CurrentHighestBidder = "Start Bid";
        }

        public void Start()
        {
            IsActive = true;
        }

        public void Close()
        {
            IsActive = false;
        }

        public void PlaceBid(decimal amount, string bidderName)
        {
            if (!IsActive)
            {
                throw new InvalidOperationException("Auction is not active.");
            }

            if (amount <= CurrentHighestBid)
            {
                throw new InvalidOperationException("Bid amount must be higher than the current highest bid.");
            }

            CurrentHighestBid = amount;
            CurrentHighestBidder = bidderName;
        }
    }
}
