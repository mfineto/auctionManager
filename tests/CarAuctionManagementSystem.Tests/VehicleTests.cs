namespace CarAuctionManagementSystem.Tests
{
    using CarAuctionManagementSystem.Models;
    using Xunit;

    public class VehicleTests
    {
        [Fact]
        public void Sedan_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            var uniqueIdentifier = "123";
            var manufacturer = "Mercedes";
            var model = "A180";
            var year = 2022;
            decimal startingBid = 15000;
            var numberOfDoors = 4;

            // Act
            var sedan = new Sedan(uniqueIdentifier, manufacturer, model, year, startingBid, numberOfDoors);

            // Assert
            Assert.Equal(uniqueIdentifier, sedan.UniqueIdentifier);
            Assert.Equal(manufacturer, sedan.Manufacturer);
            Assert.Equal(model, sedan.Model);
            Assert.Equal(year, sedan.Year);
            Assert.Equal(startingBid, sedan.StartingBid);
            Assert.Equal(numberOfDoors, sedan.NumberOfDoors);
        }

        [Fact]
        public void SUV_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            var uniqueIdentifier = "456";
            var manufacturer = "Fiat";
            var model = "Uno";
            var year = 2021;
            decimal startingBid = 25000;
            var numberOfSeats = 7;

            // Act
            var suv = new SUV(uniqueIdentifier, manufacturer, model, year, startingBid, numberOfSeats);

            // Assert
            Assert.Equal(uniqueIdentifier, suv.UniqueIdentifier);
            Assert.Equal(manufacturer, suv.Manufacturer);
            Assert.Equal(model, suv.Model);
            Assert.Equal(year, suv.Year);
            Assert.Equal(startingBid, suv.StartingBid);
            Assert.Equal(numberOfSeats, suv.NumberOfSeats);
        }

        [Fact]
        public void Truck_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            var uniqueIdentifier = "789";
            var manufacturer = "Chevrolet";
            var model = "Silverado";
            var year = 2020;
            decimal startingBid = 30000;
            var loadCapacity = 15000;

            // Act
            var truck = new Truck(uniqueIdentifier, manufacturer, model, year, startingBid, loadCapacity);

            // Assert
            Assert.Equal(uniqueIdentifier, truck.UniqueIdentifier);
            Assert.Equal(manufacturer, truck.Manufacturer);
            Assert.Equal(model, truck.Model);
            Assert.Equal(year, truck.Year);
            Assert.Equal(startingBid, truck.StartingBid);
            Assert.Equal(loadCapacity, truck.LoadCapacity);
        }
    }
}
