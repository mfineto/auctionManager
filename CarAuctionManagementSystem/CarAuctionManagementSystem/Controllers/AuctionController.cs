using System;
using CarAuctionManagementSystem.Domain;
using CarAuctionManagementSystem.Models;
using ConsoleTables;

namespace CarAuctionManagementSystem.Controllers
{
    public class AuctionController
    {
        private AuctionManager auctionManager;

        public AuctionController()
        {
            auctionManager = new AuctionManager();
        }

        public void Run()
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("=============================");
                Console.WriteLine("Car Auction Management System");
                Console.WriteLine("1. Add Vehicle");
                Console.WriteLine("2. Start Auction");
                Console.WriteLine("3. Close Auction");
                Console.WriteLine("4. Place Bid");
                Console.WriteLine("5. Search Vehicles");
                Console.WriteLine("6. Show Auctions");
                Console.WriteLine("7. Exit");

                Console.Write("Select an option: ");
                string input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        AddVehicle();
                        break;
                    case "2":
                        StartAuction();
                        break;
                    case "3":
                        CloseAuction();
                        break;
                    case "4":
                        PlaceBid();
                        break;
                    case "5":
                        SearchVehicles();
                        break;
                    case "6":
                        ListAuctions();
                        break;
                    case "7":
                        Console.WriteLine("Exiting...");
                        return;
                    default:
                        Console.WriteLine("Invalid option. Please try again.");
                        break;
                }
            }
        }

        private void AddVehicle()
        {
            try
            {
                Console.WriteLine("Enter vehicle details:");
                Console.Write("Unique Identifier: ");
                string uniqueIdentifier = Console.ReadLine();


                if (auctionManager.GetVehicleById(uniqueIdentifier) != null)
                {
                    Console.WriteLine("A vehicle with the same unique identifier already exists.");
                    return;
                }

                Console.Write("Manufacturer: ");
                string manufacturer = Console.ReadLine();
                Console.Write("Model: ");
                string model = Console.ReadLine();
                Console.Write("Year: ");
                int year = int.Parse(Console.ReadLine());
                Console.Write("Starting Bid: ");
                decimal startingBid = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Select vehicle type:");
                Console.WriteLine("1. Hatchback");
                Console.WriteLine("2. Sedan");
                Console.WriteLine("3. SUV");
                Console.WriteLine("4. Truck");
                Console.Write("Enter your choice: ");
                string typeInput = Console.ReadLine();
                VehicleType vehicleType;
                switch (typeInput)
                {
                    case "1":
                        vehicleType = VehicleType.Hatchback;
                        break;
                    case "2":
                        vehicleType = VehicleType.Sedan;
                        break;
                    case "3":
                        vehicleType = VehicleType.SUV;
                        break;
                    case "4":
                        vehicleType = VehicleType.Truck;
                        break;
                    default:
                        Console.WriteLine("Invalid vehicle type.");
                        return;
                }

                // Additional parameters based on vehicle type
                Dictionary<string, object> additionalParameters = new Dictionary<string, object>();
                if (vehicleType == VehicleType.Hatchback || vehicleType == VehicleType.Sedan)
                {
                    Console.Write("Number of Doors: ");
                    int numDoors = int.Parse(Console.ReadLine());
                    additionalParameters.Add("NumDoors", numDoors);
                }
                else if (vehicleType == VehicleType.SUV)
                {
                    Console.Write("Number of Seats: ");
                    int numSeats = int.Parse(Console.ReadLine());
                    additionalParameters.Add("NumSeats", numSeats);
                }
                else if (vehicleType == VehicleType.Truck)
                {
                    Console.Write("Load Capacity (tons): ");
                    decimal loadCapacity = decimal.Parse(Console.ReadLine());
                    additionalParameters.Add("LoadCapacity", loadCapacity);
                }

                auctionManager.AddVehicle(uniqueIdentifier, manufacturer, model, year, startingBid, vehicleType, additionalParameters);

                Console.WriteLine("Vehicle successfully added:");
                DisplayVehicleDetails(uniqueIdentifier);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while adding vehicle: {ex.Message}");
            }
        }

        private void StartAuction()
        {
            try
            {
                Console.Write("Enter unique identifier of the vehicle to start auction: ");
                string uniqueIdentifier = Console.ReadLine();
                auctionManager.StartAuction(uniqueIdentifier);
                Console.WriteLine("Auction opened successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while starting auction: {ex.Message}");
            }
        }

        private void CloseAuction()
        {
            try
            {
                Console.Write("Enter unique identifier of the vehicle to close auction: ");
                string uniqueIdentifier = Console.ReadLine();
                auctionManager.CloseAuction(uniqueIdentifier);
                Console.WriteLine("Auction closed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while closing auction: {ex.Message}");
            }
        }

        private void PlaceBid()
        {
            try
            {
                Console.Write("Enter unique identifier of the vehicle to place bid: ");
                string uniqueIdentifier = Console.ReadLine();
                Console.Write("Enter bid amount: ");
                decimal bidAmount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter bidder name: ");
                string bidderName = Console.ReadLine();
                auctionManager.PlaceBid(uniqueIdentifier, bidAmount, bidderName);
                Console.WriteLine("Bid placed successfully.");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while placing bid: {ex.Message}");
            }
        }

        private void SearchVehicles()
        {
            try
            {
                Console.WriteLine("Enter search criteria:");
                Console.Write("Manufacturer (leave blank to skip): ");
                string manufacturer = Console.ReadLine();
                Console.Write("Model (leave blank to skip): ");
                string model = Console.ReadLine();
                Console.Write("Year (leave blank to skip): ");
                string yearInput = Console.ReadLine();
                int? year = null;
                if (!string.IsNullOrEmpty(yearInput))
                    year = int.Parse(yearInput);
                Console.Write("Starting Bid (leave blank to skip): ");
                string startingBidInput = Console.ReadLine();
                decimal? startingBid = null;
                if (!string.IsNullOrEmpty(startingBidInput))
                    startingBid = decimal.Parse(startingBidInput);

                Console.Write("");
                Console.WriteLine("1. Hatchback");
                Console.WriteLine("2. Sedan");
                Console.WriteLine("3. SUV");
                Console.WriteLine("4. Truck");
                Console.Write("Vehicle Type (leave blank to skip): ");
                string vehicleTypeInput = Console.ReadLine();
                VehicleType? vehicleType = null;
                if (!string.IsNullOrEmpty(vehicleTypeInput))
                {
                    if (!Enum.TryParse(vehicleTypeInput, out VehicleType type))
                    {
                        Console.WriteLine("Invalid vehicle type.");
                        return;
                    }
                    vehicleType = type;
                }

                var results = auctionManager.SearchVehicles(manufacturer, model, year, startingBid);

                if (results.Count == 0)
                {
                    Console.WriteLine("No vehicles found matching the search criteria.");
                    return;
                }

                Console.WriteLine("Search Results:");
                ConsoleTable.From(results).Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while searching vehicles: {ex.Message}");
            }
        }

        private void ListAuctions()
        {
            try
            {
                Console.WriteLine("All Auctions:");

                var auctions = auctionManager.GetAuctions();

                if (auctions.Count == 0)
                {
                    Console.WriteLine("No auctions found matching the search criteria.");
                    return;
                }

                Console.WriteLine("Search Results:");
                var results = auctions.Select(auction => new
                {
                    auction.CurrentHighestBid,
                    auction.CurrentHighestBidder,
                    auction.IsActive,
                    auction.AssociatedVehicle.UniqueIdentifier,
                    auction.AssociatedVehicle.Manufacturer,
                    auction.AssociatedVehicle.Model,
                    auction.AssociatedVehicle.Year,
                    auction.AssociatedVehicle.StartingBid
                }).OrderBy(x => x.IsActive);

                ConsoleTable.From(results).Write();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error while searching vehicles: {ex.Message}");
            }
        }

        private void DisplayVehicleDetails(string uniqueIdentifier)
        {
            var vehicle = auctionManager.GetVehicleById(uniqueIdentifier);
            Console.WriteLine($"Unique Identifier: {vehicle.UniqueIdentifier}");
            Console.WriteLine($"Manufacturer: {vehicle.Manufacturer}");
            Console.WriteLine($"Model: {vehicle.Model}");
            Console.WriteLine($"Year: {vehicle.Year}");
            Console.WriteLine($"Starting Bid: {vehicle.StartingBid}");
        }
    }
}
