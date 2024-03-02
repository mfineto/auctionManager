namespace CarAuctionManagementSystem.Services
{
    using CarAuctionManagementSystem.Models;
    using Newtonsoft.Json;
    using Xunit;

    public class AuctionServiceTests
    {
        private readonly string dataFilePath = $"{Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)}\\test_vehicles{DateTime.Now:ddMMyyyy_HHmmss}.json";

        public AuctionServiceTests()
        {
            if (File.Exists(this.dataFilePath))
            {
                File.Delete(this.dataFilePath);
            }
        }

        [Fact]
        public void AddVehicle_WhenVehicleIsAdded_AddsVehicleToList()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);
            var vehicle = new Sedan("123", "Toyota", "Prius", 2022, 15000, 4);

            // Act
            auctionService.AddVehicle(vehicle);

            // Assert
            var jsonData = File.ReadAllText(this.dataFilePath);
            var vehicles = JsonConvert.DeserializeObject<List<Sedan>>(jsonData);

            Assert.Single(vehicles);
            Assert.Equal(vehicle.UniqueIdentifier, vehicles[0].UniqueIdentifier);
        }

        [Fact]
        public void AddVehicle_WhenUniqueIdentifierAlreadyExists_DoesNotAddVehicle()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);
            var sedan = new Sedan("123", "Toyota", "Prius", 2022, 15000, 4);
            auctionService.AddVehicle(sedan);

            // Act & Assert
            var ex = Assert.Throws<InvalidOperationException>(() =>
            {
                // Attempt to add vehicle with same unique identifier
                auctionService.AddVehicle(sedan);
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void AddVehicle_WhenVehicleIsAdded_SavesDataToFile()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);
            var vehicle = new Sedan("123", "VW", "Golf", 2022, 15000, 4);

            // Act
            auctionService.AddVehicle(vehicle);

            // Assert
            Assert.True(File.Exists(this.dataFilePath));
            var jsonData = File.ReadAllText(this.dataFilePath);
            Assert.False(string.IsNullOrEmpty(jsonData));

            var vehicles = JsonConvert.DeserializeObject<List<Sedan>>(jsonData);

            Assert.Single(vehicles);
            Assert.Equal(vehicle.UniqueIdentifier, vehicles[0].UniqueIdentifier);
        }

        [Fact]
        public void AddVehicle_WhenFilePathIsEmpty_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                // Attempt to create AuctionService with empty file path
                var auctionService = new AuctionService("");
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void AddVehicle_WhenFilePathIsInvalid_ThrowsException()
        {
            // Act & Assert
            var ex = Assert.Throws<ArgumentException>(() =>
            {
                // Attempt to create AuctionService with invalid file path
                var auctionService = new AuctionService("invalid\\path\\test_vehicles.json");
            });

            Assert.NotNull(ex);
        }

        [Fact]
        public void SearchVehicles_WhenPredicateIsNull_ReturnsAllVehicles()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);
            var sedan = new Sedan("123", "Honda", "Civic", 2022, 15000, 4);
            var suv = new SUV("456", "Honda", "CR-V", 2021, 20000, 5);
            auctionService.AddVehicle(sedan);
            auctionService.AddVehicle(suv);

            // Act
            var searchResult = auctionService.SearchVehicles(null);

            // Assert
            Assert.Equal(2, searchResult.Count);
        }

        [Fact]
        public void SearchVehicles_WhenNoMatchFound_ReturnsEmptyList()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);
            var sedan = new Sedan("123", "VW", "Golf", 2022, 15000, 4);
            auctionService.AddVehicle(sedan);

            // Act
            var searchResult = auctionService.SearchVehicles(v => v.Manufacturer == "Honda");

            // Assert
            Assert.Empty(searchResult);
        }

        [Fact]
        public void SearchVehicles_WhenNoVehiclesAdded_ReturnsEmptyList()
        {
            // Arrange
            var auctionService = new AuctionService(this.dataFilePath);

            // Act
            var searchResult = auctionService.SearchVehicles(v => v.Manufacturer == "Toyota");

            // Assert
            Assert.Empty(searchResult);
        }
    }
}
