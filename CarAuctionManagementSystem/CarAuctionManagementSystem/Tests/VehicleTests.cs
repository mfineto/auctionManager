using NUnit.Framework;
using CarAuctionManagementSystem.Domain;
using CarAuctionManagementSystem.Models;
using System;

namespace CarAuctionManagementSystem.Tests
{
    [TestFixture]
    public class VehicleTests
    {
        [Test]
        public void Sedan_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            string uniqueIdentifier = "123";
            string manufacturer = "Mercedes";
            string model = "A180";
            int year = 2022;
            decimal startingBid = 15000;
            int numberOfDoors = 4;

            // Act
            var sedan = new Sedan(uniqueIdentifier, manufacturer, model, year, startingBid, numberOfDoors);

            // Assert
            Assert.That(uniqueIdentifier, Is.EqualTo(sedan.UniqueIdentifier));
            Assert.That(manufacturer, Is.EqualTo(sedan.Manufacturer));
            Assert.That(model, Is.EqualTo(sedan.Model));
            Assert.That(year, Is.EqualTo(sedan.Year));
            Assert.That(startingBid, Is.EqualTo(sedan.StartingBid));
            Assert.That(numberOfDoors, Is.EqualTo(sedan.NumberOfDoors));
        }

        [Test]
        public void SUV_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            string uniqueIdentifier = "456";
            string manufacturer = "Fiat";
            string model = "Uno";
            int year = 2021;
            decimal startingBid = 25000;
            int numberOfSeats = 7;

            // Act
            var suv = new SUV(uniqueIdentifier, manufacturer, model, year, startingBid, numberOfSeats);

            // Assert
            Assert.That(uniqueIdentifier, Is.EqualTo(suv.UniqueIdentifier));
            Assert.That(manufacturer, Is.EqualTo(suv.Manufacturer));
            Assert.That(model, Is.EqualTo(suv.Model));
            Assert.That(year, Is.EqualTo(suv.Year));
            Assert.That(startingBid, Is.EqualTo(suv.StartingBid));
            Assert.That(numberOfSeats, Is.EqualTo(suv.NumberOfSeats));
        }

        [Test]
        public void Truck_Constructor_CreatesInstanceWithCorrectValues()
        {
            // Arrange
            string uniqueIdentifier = "789";
            string manufacturer = "Chevrolet";
            string model = "Silverado";
            int year = 2020;
            decimal startingBid = 30000;
            int loadCapacity = 15000;

            // Act
            var truck = new Truck(uniqueIdentifier, manufacturer, model, year, startingBid, loadCapacity);

            // Assert;
            Assert.That(uniqueIdentifier, Is.EqualTo(truck.UniqueIdentifier));
            Assert.That(manufacturer, Is.EqualTo(truck.Manufacturer));
            Assert.That(model, Is.EqualTo(truck.Model));
            Assert.That(year, Is.EqualTo(truck.Year));
            Assert.That(startingBid, Is.EqualTo(truck.StartingBid));
            Assert.That(loadCapacity, Is.EqualTo(truck.LoadCapacity));
        }
    }
}
