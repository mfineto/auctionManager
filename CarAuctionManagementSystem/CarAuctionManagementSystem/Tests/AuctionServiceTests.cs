using CarAuctionManagementSystem.Models;
using CarAuctionManagementSystem.Services;
using Newtonsoft.Json;
using NUnit.Framework;

namespace CarAuctionManagementSystem.Tests
{
    [TestFixture]
    public class AuctionServiceTests
    {
        private string DataFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\test_vehicles{DateTime.Now.ToString("ddMMyyyy_HHmmss")}.json";

        [SetUp]
        public void Setup()
        {
            if (File.Exists(DataFilePath))
                File.Delete(DataFilePath);
        }

        [Test]
        public void AddVehicle_AddsVehicleToList_WhenVehicleIsAdded()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);
            var vehicle = new Sedan("123", "Toyota", "Prius", 2022, 15000, 4);

            // Act
            auctionService.AddVehicle(vehicle);

            // Assert
            var jsonData = File.ReadAllText(DataFilePath);
            var vehicles = JsonConvert.DeserializeObject<List<Sedan>>(jsonData);

            Assert.That(vehicles.Count, Is.EqualTo(1));
            Assert.That(vehicle.UniqueIdentifier, Is.EqualTo(vehicles[0].UniqueIdentifier));
        }

        [Test]
        public void AddVehicle_DoesNotAddVehicle_WhenUniqueIdentifierAlreadyExists()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);
            var sedan = new Sedan("123", "Toyota", "Prius", 2022, 15000, 4);
            auctionService.AddVehicle(sedan);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                // Attempt to add vehicle with same unique identifier
                auctionService.AddVehicle(sedan);
            });

            Assert.That(ex, Is.Not.Null);
        }

        [Test]
        public void AddVehicle_SavesDataToFile()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);
            var vehicle = new Sedan("123", "VW", "Golf", 2022, 15000, 4);

            // Act
            auctionService.AddVehicle(vehicle);

            // Assert
            Assert.Multiple(() =>
            {
                Assert.That(File.Exists(DataFilePath), Is.True);
                var jsonData = File.ReadAllText(DataFilePath);
                Assert.That(string.IsNullOrEmpty(jsonData), Is.False);

                var vehicles = JsonConvert.DeserializeObject<List<Sedan>>(jsonData);

                Assert.That(vehicles.Count, Is.EqualTo(1));
                Assert.That(vehicle.UniqueIdentifier, Is.EqualTo(vehicles[0].UniqueIdentifier));
            });
        }

        [Test]
        public void AddVehicle_ThrowsException_WhenFilePathIsEmpty()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                // Attempt to create AuctionService with empty file path
                var auctionService = new AuctionService("");
            });

            Assert.That(ex, Is.Not.Null);
        }

        [Test]
        public void AddVehicle_ThrowsException_WhenFilePathIsInvalid()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                // Attempt to create AuctionService with invalid file path
                var auctionService = new AuctionService("invalid\\path\\test_vehicles.json");
            });

            Assert.That(ex, Is.Not.Null);
        }

        [Test]
        public void SearchVehicles_ReturnsAllVehicles_WhenPredicateIsNull()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);
            var sedan = new Sedan("123", "Honda", "Civic", 2022, 15000, 4);
            var suv = new SUV("456", "Honda", "CR-V", 2021, 20000, 5);
            auctionService.AddVehicle(sedan);
            auctionService.AddVehicle(suv);

            // Act
            var searchResult = auctionService.SearchVehicles(null);

            // Assert
            Assert.That(searchResult.Count, Is.EqualTo(2));

        }

        [Test]
        public void SearchVehicles_ReturnsEmptyList_WhenNoMatchFound()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);
            var sedan = new Sedan("123", "VW", "Golf", 2022, 15000, 4);
            auctionService.AddVehicle(sedan);

            // Act
            var searchResult = auctionService.SearchVehicles(v => v.Manufacturer == "Honda");

            // Assert
            Assert.That(searchResult, Is.Empty);
        }

        [Test]
        public void SearchVehicles_ReturnsEmptyList_WhenNoVehiclesAdded()
        {
            // Arrange
            var auctionService = new AuctionService(DataFilePath);

            // Act
            var searchResult = auctionService.SearchVehicles(v => v.Manufacturer == "Toyota");

            // Assert
            Assert.That(searchResult, Is.Empty);
        }
    }
}
