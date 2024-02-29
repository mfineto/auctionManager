using CarAuctionManagementSystem.Domain;
using CarAuctionManagementSystem.Models;
using NUnit.Framework;

namespace CarAuctionManagementSystem.Tests
{
    [TestFixture]
    public class AuctionTests
    {
        private IVehicle vehicle;
        private decimal startingBid;

        [SetUp]
        public void Setup()
        {
            vehicle = new Sedan("1", "Fiat", "Uno", 2022, 25000, 4);
            startingBid = 20000;
        }

        [Test]
        public void Constructor_Initialization_PropertiesSetCorrectly()
        {
            // Arrange & Act
            Auction auction = new Auction(vehicle, startingBid);

            // Assert
            Assert.That(auction.AssociatedVehicle, Is.EqualTo(vehicle));
            Assert.That(auction.IsActive, Is.False);
            Assert.That(auction.CurrentHighestBid, Is.EqualTo(startingBid));
            Assert.That(auction.CurrentHighestBidder, Is.EqualTo("Start Bid"));
        }

        [Test]
        public void Start_AuctionIsNotActive_AuctionIsActive()
        {
            // Arrange
            Auction auction = new Auction(vehicle, startingBid);

            // Act
            auction.Start();

            // Assert
            Assert.That(auction.IsActive, Is.True);
        }

        [Test]
        public void Close_AuctionIsActive_AuctionIsNotActive()
        {
            // Arrange
            Auction auction = new Auction(vehicle, startingBid);
            auction.Start();

            // Act
            auction.Close();

            // Assert
            Assert.That(auction.IsActive, Is.False);
        }

        [Test]
        public void PlaceBid_ValidBid_PlacesBid()
        {
            // Arrange
            Auction auction = new Auction(vehicle, startingBid);
            auction.Start();
            decimal newBid = startingBid + 1000;
            string bidderName = "John";

            // Act
            auction.PlaceBid(newBid, bidderName);

            // Assert
            Assert.That(auction.CurrentHighestBid, Is.EqualTo(newBid));
            Assert.That(auction.CurrentHighestBidder, Is.EqualTo(bidderName));
        }

        [Test]
        public void PlaceBid_InvalidBid_ThrowsInvalidOperationException()
        {
            // Arrange
            Auction auction = new Auction(vehicle, startingBid);
            auction.Start();
            decimal newBid = startingBid - 1000;
            string bidderName = "John";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(newBid, bidderName));
        }

        [Test]
        public void PlaceBid_AuctionNotStarted_ThrowsInvalidOperationException()
        {
            // Arrange
            Auction auction = new Auction(vehicle, startingBid);
            decimal newBid = startingBid + 1000;
            string bidderName = "John";

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auction.PlaceBid(newBid, bidderName));
        }
    }
}
