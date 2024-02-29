using CarAuctionManagementSystem.Domain;
using CarAuctionManagementSystem.Models;
using NUnit.Framework;

namespace CarAuctionManagementSystem.Tests
{
    [TestFixture]
    public class AuctionManagerTests
    {
        private AuctionManager auctionManager;

        [SetUp]
        public void Setup()
        {
            auctionManager = new AuctionManager();

            auctionManager.AddVehicle("1", "Mercedes", "C-Class", 2020, 10000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            auctionManager.AddVehicle("2", "Mercedes", "E-Class", 2018, 12000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            auctionManager.AddVehicle("3", "Volkswagen", "Tiguan", 2019, 30000, VehicleType.SUV, new Dictionary<string, object> { { "NumSeats", 5 } });
            auctionManager.AddVehicle("4", "Volkswagen", "Golf", 2020, 25000, VehicleType.Hatchback, new Dictionary<string, object> { { "NumDoors", 4 } });
            auctionManager.AddVehicle("5", "Ford", "F-150", 2017, 40000, VehicleType.Truck, new Dictionary<string, object> { { "LoadCapacity", 1000 } });
        }

        [Test]
        public void AddVehicle_WhenUniqueIdentifierAlreadyExists_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                auctionManager.AddVehicle("1", "Mercedes", "C-Class", 2020, 35000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            });
        }

        [Test]
        public void AddVehicle_WhenParametersAreValid_AddsVehicle()
        {
            // Act
            auctionManager.AddVehicle("6", "Mercedes", "C-Class", 2020, 35000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });

            // Assert
            var vehicle = (Sedan)auctionManager.GetVehicleById("6");
            Assert.That(vehicle, Is.Not.Null);
            Assert.That(vehicle.Manufacturer, Is.EqualTo("Mercedes"));
            Assert.That(vehicle.Model, Is.EqualTo("C-Class"));
            Assert.That(vehicle.Year, Is.EqualTo(2020));
            Assert.That(vehicle.StartingBid, Is.EqualTo(35000));
        }


        [Test]
        public void CloseAuction_WhenAuctionAlreadyClosed_ThrowsException()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.CloseAuction("1");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.CloseAuction("1");
            });
        }

        [Test]
        public void CloseAuction_WhenVehicleIdIsEmpty_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.CloseAuction("");
            });
        }

        [Test]
        public void CloseAuction_WhenVehicleIdIsNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.CloseAuction(null);
            });
        }

        [Test]
        public void GetAuction_WhenAuctionStartedForVehicle_ReturnsAuction()
        {
            // Arrange
            auctionManager.StartAuction("1");

            // Act
            var auction = auctionManager.GetAuction("1");

            // Assert
            Assert.That(auction, Is.Not.Null);
        }

        [Test]
        public void GetAuction_WhenMultipleAuctionsStarted_ReturnsCorrectAuction()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.StartAuction("2");

            // Act
            var auction1 = auctionManager.GetAuction("1");
            var auction2 = auctionManager.GetAuction("2");

            // Assert
            Assert.That(auction1, Is.Not.Null);
            Assert.That(auction2, Is.Not.Null);
            Assert.That(auction1, Is.Not.EqualTo(auction2));
        }

        [Test]
        public void GetAuction_WhenSameAuctionStartedTwice_ReturnsCorrectAuction()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.PlaceBid("1", 20000, "Jose");
            var initialAuction = auctionManager.GetAuction("1");
            auctionManager.CloseAuction("1");

            // Act
            auctionManager.StartAuction("1");
            var newAuction = auctionManager.GetAuction("1");

            // Assert
            Assert.That(initialAuction, Is.Not.Null);
            Assert.That(newAuction, Is.Not.Null);
            Assert.That(initialAuction, Is.EqualTo(newAuction));
        }

        [Test]
        public void GetAuction_WhenAuctionClosedAndNewAuctionStarted_ReturnsCorrectAuction()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.CloseAuction("1");

            // Act
            auctionManager.StartAuction("1");
            var newAuction = auctionManager.GetAuction("1");

            // Assert
            Assert.That(newAuction, Is.Not.Null);
            Assert.That(newAuction.IsActive, Is.True);
        }

        [Test]
        public void GetAuction_WhenNoAuctionStartedForVehicle_ReturnsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.GetAuction("123");
            });
        }

        [Test]
        public void GetAuction_WhenVehicleIdIsEmpty_ReturnsInvalidOperationException()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.GetAuction("");
            });
        }

        [Test]
        public void GetAuction_WhenVehicleIdIsNull_ReturnsNullException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.GetAuction(null);
            });
        }

        [Test]
        public void PlaceBid_WhenVehicleIsValid_SuccessfulBid()
        {
            // Arrange
            auctionManager.StartAuction("1");

            // Act
            auctionManager.PlaceBid("1", 30000, "Daniel");

            // Assert
            Assert.That(auctionManager.GetHighestBid("1"), Is.EqualTo(30000));
        }

        [Test]
        public void PlaceBid_AuctionNotActive_UnsuccessfulBid()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.CloseAuction("1");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("1", 1500, "Maria"));
        }

        [Test]
        public void PlaceBid_InvalidBidAmount_UnsuccessfulBid()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.PlaceBid("1", 25000, "John");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("1", 1500, "Jose"));
        }

        [Test]
        public void PlaceBid_WhenVehicleNotExists_UnsuccessfulBid()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("9999", 1500, "Michael"));
        }

        [Test]
        public void PlaceBid_InvalidVehicleId_UnsuccessfulBid()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => auctionManager.PlaceBid(null, 1500, "Peter"));
        }

        [Test]
        public void SearchVehicles_WhenNoCriteriaProvided_ReturnsAllVehicles()
        {
            // Act
            var result = auctionManager.SearchVehicles();

            // Assert
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void SearchVehicles_WhenInvalidManufacturerIsProvided_ReturnsZeroVehicles()
        {
            // Act
            var result = auctionManager.SearchVehicles(manufacturer: "XYZ");

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void SearchVehicles_WhenInvalidModelIsProvided_ReturnsZeroVehicles()
        {
            // Act
            var result = auctionManager.SearchVehicles(model: "XYZ");

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void SearchVehicles_WhenInvalidYearIsProvided_ReturnsZeroVehicles()
        {
            // Act
            var result = auctionManager.SearchVehicles(year: 3000);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void StartAuction_WhenAuctionAlreadyStartedForVehicle_ThrowsException()
        {
            // Arrange
            auctionManager.StartAuction("1");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.StartAuction("1");
            });
        }

        [Test]
        public void StartAuction_WhenVehicleIdIsEmpty_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.StartAuction("");
            });
        }

        [Test]
        public void StartAuction_WhenVehicleIdIsNull_ThrowsException()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.StartAuction(null);
            });
        }

        [Test]
        public void StartAuction_WithValidVehicleId_StartsAuction()
        {
            // Act
            auctionManager.StartAuction("5");

            // Assert
            Assert.That(auctionManager.GetAuction("5").IsActive, Is.True);
        }
    }
}
