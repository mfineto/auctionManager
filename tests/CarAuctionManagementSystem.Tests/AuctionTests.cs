namespace CarAuctionManagementSystem.Tests
{
    using System;
    using CarAuctionManagementSystem.Domain;
    using CarAuctionManagementSystem.Models;
    using Xunit;

    public class AuctionTests
    {
        private readonly IVehicle vehicle;
        private readonly decimal startingBid;

        public AuctionTests()
        {
            this.vehicle = new Sedan("1", "Fiat", "Uno", 2022, 25000, 4);
            this.startingBid = 20000;
        }

        [Fact]
        public void Constructor_Initialization_PropertiesSetCorrectly()
        {
            // Arrange & Act
            var auction = new Auction(this.vehicle, this.startingBid);

            // Assert
            Assert.Equal(this.vehicle, auction.AssociatedVehicle);
            Assert.False(auction.IsActive);
            Assert.Equal(this.startingBid, auction.CurrentHighestBid);
            Assert.Equal("Start Bid", auction.CurrentHighestBidder);
        }

        [Fact]
        public void Start_AuctionIsNotActive_AuctionIsActive()
        {
            // Arrange
            var auction = new Auction(this.vehicle, this.startingBid);

            // Act
            auction.Start();

            // Assert
            Assert.True(auction.IsActive);
        }

        [Fact]
        public void Close_AuctionIsActive_AuctionIsNotActive()
        {
            // Arrange
            var auction = new Auction(this.vehicle, this.startingBid);
            auction.Start();

            // Act
            auction.Close();

            // Assert
            Assert.False(auction.IsActive);
        }

        [Fact]
        public void PlaceBid_ValidBid_PlacesBid()
        {
            // Arrange
            var auction = new Auction(this.vehicle, this.startingBid);
            auction.Start();

            var newBid = this.startingBid + 1000;
            var bidderName = "John";

            // Act
            auction.PlaceBid(newBid, bidderName);

            // Assert
            Assert.Equal(newBid, auction.CurrentHighestBid);
            Assert.Equal(bidderName, auction.CurrentHighestBidder);
        }

        [Fact]
        public void PlaceBid_InvalidBid_ThrowsInvalidOperationException()
        {
            // Arrange
            var auction = new Auction(this.vehicle, this.startingBid);
            auction.Start();

            var newBid = this.startingBid - 1000;
            var bidderName = "John";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(newBid, bidderName));
        }

        [Fact]
        public void PlaceBid_AuctionNotStarted_ThrowsInvalidOperationException()
        {
            // Arrange
            var auction = new Auction(this.vehicle, this.startingBid);
            var newBid = this.startingBid + 1000;
            var bidderName = "John";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(newBid, bidderName));
        }
    }
}
