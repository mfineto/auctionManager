namespace CarAuctionManagementSystem.Tests
{
    using System;
    using System.Collections.Generic;
    using CarAuctionManagementSystem.Domain;
    using CarAuctionManagementSystem.Models;
    using Xunit;

    public class AuctionManagerTests
    {
        private readonly AuctionManager auctionManager;

        public AuctionManagerTests()
        {
            this.auctionManager = new AuctionManager();

            this.auctionManager.AddVehicle("1", "Mercedes", "C-Class", 2020, 10000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            this.auctionManager.AddVehicle("2", "Mercedes", "E-Class", 2018, 12000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            this.auctionManager.AddVehicle("3", "Volkswagen", "Tiguan", 2019, 30000, VehicleType.SUV, new Dictionary<string, object> { { "NumSeats", 5 } });
            this.auctionManager.AddVehicle("4", "Volkswagen", "Golf", 2020, 25000, VehicleType.Hatchback, new Dictionary<string, object> { { "NumDoors", 4 } });
            this.auctionManager.AddVehicle("5", "Ford", "F-150", 2017, 40000, VehicleType.Truck, new Dictionary<string, object> { { "LoadCapacity", 1000 } });
        }

        [Fact]
        public void AddVehicle_WhenUniqueIdentifierAlreadyExists_ThrowsException()
        {
            Assert.Throws<ArgumentException>(() =>
                this.auctionManager.AddVehicle("1", "Mercedes", "C-Class", 2020, 35000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } }));
        }

        [Fact]
        public void AddVehicle_WhenParametersAreValid_AddsVehicle()
        {
            this.auctionManager.AddVehicle("6", "Mercedes", "C-Class", 2020, 35000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });

            var vehicle = (Sedan)this.auctionManager.GetVehicleById("6");
            Assert.NotNull(vehicle);
            Assert.Equal("Mercedes", vehicle.Manufacturer);
            Assert.Equal("C-Class", vehicle.Model);
            Assert.Equal(2020, vehicle.Year);
            Assert.Equal(35000, vehicle.StartingBid);
        }

        [Fact]
        public void CloseAuction_WhenAuctionAlreadyClosed_ThrowsException()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.CloseAuction("1");

            Assert.Throws<InvalidOperationException>(() => this.auctionManager.CloseAuction("1"));
        }

        [Fact]
        public void CloseAuction_WhenVehicleIdIsEmpty_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.CloseAuction(""));
        }

        [Fact]
        public void CloseAuction_WhenVehicleIdIsNull_ThrowsException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.CloseAuction(null));
        }

        [Fact]
        public void GetAuction_WhenAuctionStartedForVehicle_ReturnsAuction()
        {
            this.auctionManager.StartAuction("1");

            var auction = this.auctionManager.GetAuction("1");
            Assert.NotNull(auction);
        }

        [Fact]
        public void GetAuction_WhenMultipleAuctionsStarted_ReturnsCorrectAuction()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.StartAuction("2");

            var auction1 = this.auctionManager.GetAuction("1");
            var auction2 = this.auctionManager.GetAuction("2");

            Assert.NotNull(auction1);
            Assert.NotNull(auction2);
            Assert.NotEqual(auction1, auction2);
        }

        [Fact]
        public void GetAuction_WhenSameAuctionStartedTwice_ReturnsCorrectAuction()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.PlaceBid("1", 20000, "Jose");
            var initialAuction = this.auctionManager.GetAuction("1");
            this.auctionManager.CloseAuction("1");

            this.auctionManager.StartAuction("1");
            var newAuction = this.auctionManager.GetAuction("1");

            Assert.NotNull(initialAuction);
            Assert.NotNull(newAuction);
            Assert.Equal(initialAuction, newAuction);
        }

        [Fact]
        public void GetAuction_WhenAuctionClosedAndNewAuctionStarted_ReturnsCorrectAuction()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.CloseAuction("1");

            this.auctionManager.StartAuction("1");
            var newAuction = this.auctionManager.GetAuction("1");

            Assert.NotNull(newAuction);
            Assert.True(newAuction.IsActive);
        }

        [Fact]
        public void GetAuction_WhenNoAuctionStartedForVehicle_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.auctionManager.GetAuction("123"));
        }

        [Fact]
        public void GetAuction_WhenVehicleIdIsEmpty_ThrowsInvalidOperationException()
        {
            Assert.Throws<InvalidOperationException>(() => this.auctionManager.GetAuction(""));
        }

        [Fact]
        public void GetAuction_WhenVehicleIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.GetAuction(null));
        }

        [Fact]
        public void PlaceBid_WhenVehicleIsValid_SuccessfulBid()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.PlaceBid("1", 30000, "Daniel");

            Assert.Equal(30000, this.auctionManager.GetHighestBid("1"));
        }

        [Fact]
        public void PlaceBid_AuctionNotActive_UnsuccessfulBid()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.CloseAuction("1");

            Assert.Throws<InvalidOperationException>(() => this.auctionManager.PlaceBid("1", 1500, "Maria"));
        }

        [Fact]
        public void PlaceBid_InvalidBidAmount_UnsuccessfulBid()
        {
            this.auctionManager.StartAuction("1");
            this.auctionManager.PlaceBid("1", 25000, "John");

            Assert.Throws<InvalidOperationException>(() => this.auctionManager.PlaceBid("1", 1500, "Jose"));
        }

        [Fact]
        public void PlaceBid_WhenVehicleNotExists_UnsuccessfulBid()
        {
            Assert.Throws<InvalidOperationException>(() => this.auctionManager.PlaceBid("9999", 1500, "Michael"));
        }

        [Fact]
        public void PlaceBid_InvalidVehicleId_UnsuccessfulBid()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.PlaceBid(null, 1500, "Peter"));
        }

        [Fact]
        public void SearchVehicles_WhenNoCriteriaProvided_ReturnsAllVehicles()
        {
            var result = this.auctionManager.SearchVehicles();

            Assert.Equal(5, result.Count);
        }

        [Fact]
        public void SearchVehicles_WhenInvalidManufacturerIsProvided_ReturnsZeroVehicles()
        {
            var result = this.auctionManager.SearchVehicles(manufacturer: "XYZ");

            Assert.Empty(result);
        }

        [Fact]
        public void SearchVehicles_WhenInvalidModelIsProvided_ReturnsZeroVehicles()
        {
            var result = this.auctionManager.SearchVehicles(model: "XYZ");

            Assert.Empty(result);
        }

        [Fact]
        public void SearchVehicles_WhenInvalidYearIsProvided_ReturnsZeroVehicles()
        {
            var result = this.auctionManager.SearchVehicles(year: 3000);

            Assert.Empty(result);
        }

        [Fact]
        public void SearchVehicles_WhenValidManufacturer_ReturnsVehicles()
        {
            var result = this.auctionManager.SearchVehicles(manufacturer: "Mercedes");

            Assert.True(result.Any());
        }

        [Fact]
        public void SearchVehicles_WhenValidVehicleType_ReturnsVehicles()
        {
            var result = this.auctionManager.SearchVehicles(vehicleType: VehicleType.Truck);

            Assert.True(result.Any());
        }

        [Fact]
        public void StartAuction_WhenAuctionAlreadyStartedForVehicle_ThrowsException()
        {
            this.auctionManager.StartAuction("1");

            Assert.Throws<InvalidOperationException>(() => this.auctionManager.StartAuction("1"));
        }

        [Fact]
        public void StartAuction_WhenVehicleIdIsEmpty_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.StartAuction(""));
        }

        [Fact]
        public void StartAuction_WhenVehicleIdIsNull_ThrowsArgumentNullException()
        {
            Assert.Throws<ArgumentNullException>(() => this.auctionManager.StartAuction(null));
        }

        [Fact]
        public void StartAuction_WithValidVehicleId_StartsAuction()
        {
            this.auctionManager.StartAuction("5");

            var auction = this.auctionManager.GetAuction("5");
            Assert.True(auction.IsActive);
        }
    }
}
