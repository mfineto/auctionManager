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
        public void AddVehicle_ThrowsException_WhenUniqueIdentifierAlreadyExists()
        {
            // Act & Assert
            Assert.Throws<ArgumentException>(() =>
            {
                auctionManager.AddVehicle("1", "Mercedes", "C-Class", 2020, 35000, VehicleType.Sedan, new Dictionary<string, object> { { "NumDoors", 4 } });
            });
        }

        [Test]
        public void AddVehicle_AddsVehicle_WhenParametersAreValid()
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
        public void CloseAuction_ThrowsException_WhenAuctionAlreadyClosed()
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
        public void CloseAuction_ThrowsException_WhenVehicleIdIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.CloseAuction("");
            });
        }

        [Test]
        public void CloseAuction_ThrowsException_WhenVehicleIdIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.CloseAuction(null);
            });
        }

        [Test]
        public void GetAuction_ReturnsAuction_WhenAuctionStartedForVehicle()
        {
            // Arrange
            auctionManager.StartAuction("1");

            // Act
            var auction = auctionManager.GetAuction("1");

            // Assert
            Assert.That(auction, Is.Not.Null);
        }

        [Test]
        public void GetAuction_ReturnsCorrectAuction_WhenMultipleAuctionsStarted()
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
        public void GetAuction_ReturnsCorrectAuction_WhenSameAuctionStartedTwice()
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
        public void GetAuction_ReturnsCorrectAuction_WhenAuctionClosedAndNewAuctionStarted()
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
        public void GetAuction_ReturnsNull_WhenNoAuctionStartedForVehicle()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.GetAuction("123");
            });
        }

        [Test]
        public void GetAuction_ReturnsNull_WhenVehicleIdIsEmpty()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() =>
            {
                auctionManager.GetAuction("");
            });
        }

        [Test]
        public void GetAuction_ReturnsNullException_WhenVehicleIdIsNull()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.GetAuction(null);
            });
        }

        [Test]
        public void PlaceBid_SuccessfulBid_WhenVehicleIsValid()
        {
            // Arrange
            auctionManager.StartAuction("1");

            // Act
            auctionManager.PlaceBid("1", 30000, "Daniel");

            // Assert
            Assert.That(auctionManager.GetHighestBid("1"), Is.EqualTo(30000));
        }

        [Test]
        public void PlaceBid_UnsuccessfulBid_AuctionNotActive()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.CloseAuction("1");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("1", 1500, "Maria"));
        }

        [Test]
        public void PlaceBid_UnsuccessfulBid_InvalidBidAmount()
        {
            // Arrange
            auctionManager.StartAuction("1");
            auctionManager.PlaceBid("1", 25000, "John");

            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("1", 1500, "Jose"));
        }

        [Test]
        public void PlaceBid_UnsuccessfulBid_VehicleNotFound()
        {
            // Act & Assert
            Assert.Throws<InvalidOperationException>(() => auctionManager.PlaceBid("vehicle4", 1500, "Michael"));
        }

        [Test]
        public void PlaceBid_UnsuccessfulBid_InvalidVehicleId()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() => auctionManager.PlaceBid(null, 1500, "Peter"));
        }

        [Test]
        public void SearchVehicles_ReturnsAllVehicles_WhenNoCriteriaProvided()
        {
            // Act
            var result = auctionManager.SearchVehicles();

            // Assert
            Assert.That(result.Count, Is.EqualTo(5));
        }

        [Test]
        public void SearchVehicles_ReturnsAllVehicles_WhenInvalidManufacturerIsProvided()
        {
            // Act
            var result = auctionManager.SearchVehicles(manufacturer: "XYZ");

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void SearchVehicles_ReturnsAllVehicles_WhenInvalidModelIsProvided()
        {
            // Act
            var result = auctionManager.SearchVehicles(model: "XYZ");

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void SearchVehicles_ReturnsAllVehicles_WhenInvalidYearIsProvided()
        {
            // Act
            var result = auctionManager.SearchVehicles(year: 3000);

            // Assert
            Assert.That(result.Count, Is.EqualTo(0));
        }

        [Test]
        public void StartAuction_ThrowsException_WhenAuctionAlreadyStartedForVehicle()
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
        public void StartAuction_ThrowsException_WhenVehicleIdIsEmpty()
        {
            // Act & Assert
            Assert.Throws<ArgumentNullException>(() =>
            {
                auctionManager.StartAuction("");
            });
        }

        [Test]
        public void StartAuction_ThrowsException_WhenVehicleIdIsNull()
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
