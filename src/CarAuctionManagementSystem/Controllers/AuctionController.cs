namespace CarAuctionManagementSystem.Controllers
{
    using CarAuctionManagementSystem.Domain;
    using ConsoleTables;

    public class AuctionController
    {
        private readonly AuctionManager auctionManager;

        public AuctionController()
        {
            this.auctionManager = new AuctionManager();
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
                var input = Console.ReadLine();

                switch (input)
                {
                    case "1":
                        this.AddVehicle();
                        break;
                    case "2":
                        this.StartAuction();
                        break;
                    case "3":
                        this.CloseAuction();
                        break;
                    case "4":
                        this.PlaceBid();
                        break;
                    case "5":
                        this.SearchVehicles();
                        break;
                    case "6":
                        this.ListAuctions();
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
                var uniqueIdentifier = Console.ReadLine();

                if (this.auctionManager.GetVehicleById(uniqueIdentifier) != null)
                {
                    Console.WriteLine("A vehicle with the same unique identifier already exists.");
                    return;
                }

                Console.Write("Manufacturer: ");
                var manufacturer = Console.ReadLine();
                Console.Write("Model: ");
                var model = Console.ReadLine();
                Console.Write("Year: ");
                var year = int.Parse(Console.ReadLine());
                Console.Write("Starting Bid: ");
                var startingBid = decimal.Parse(Console.ReadLine());
                Console.WriteLine("Select vehicle type:");
                Console.WriteLine("1. Hatchback");
                Console.WriteLine("2. Sedan");
                Console.WriteLine("3. SUV");
                Console.WriteLine("4. Truck");
                Console.Write("Enter your choice: ");
                var typeInput = Console.ReadLine();
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
                var additionalParameters = new Dictionary<string, object>();
                if (vehicleType == VehicleType.Hatchback || vehicleType == VehicleType.Sedan)
                {
                    Console.Write("Number of Doors: ");
                    var numDoors = int.Parse(Console.ReadLine());
                    additionalParameters.Add("NumDoors", numDoors);
                }
                else if (vehicleType == VehicleType.SUV)
                {
                    Console.Write("Number of Seats: ");
                    var numSeats = int.Parse(Console.ReadLine());
                    additionalParameters.Add("NumSeats", numSeats);
                }
                else if (vehicleType == VehicleType.Truck)
                {
                    Console.Write("Load Capacity (tons): ");
                    var loadCapacity = decimal.Parse(Console.ReadLine());
                    additionalParameters.Add("LoadCapacity", loadCapacity);
                }

                this.auctionManager.AddVehicle(uniqueIdentifier, manufacturer, model, year, startingBid, vehicleType, additionalParameters);

                Console.WriteLine("Vehicle successfully added:");
                this.DisplayVehicleDetails(uniqueIdentifier);
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
                var uniqueIdentifier = Console.ReadLine();
                this.auctionManager.StartAuction(uniqueIdentifier);
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
                var uniqueIdentifier = Console.ReadLine();
                this.auctionManager.CloseAuction(uniqueIdentifier);
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
                var uniqueIdentifier = Console.ReadLine();
                Console.Write("Enter bid amount: ");
                var bidAmount = decimal.Parse(Console.ReadLine());
                Console.Write("Enter bidder name: ");
                var bidderName = Console.ReadLine();
                this.auctionManager.PlaceBid(uniqueIdentifier, bidAmount, bidderName);
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
                var manufacturer = Console.ReadLine();
                Console.Write("Model (leave blank to skip): ");
                var model = Console.ReadLine();
                Console.Write("Year (leave blank to skip): ");
                var yearInput = Console.ReadLine();
                int? year = null;
                if (!string.IsNullOrEmpty(yearInput))
                {
                    year = int.Parse(yearInput);
                }

                Console.Write("Starting Bid (leave blank to skip): ");
                var startingBidInput = Console.ReadLine();
                decimal? startingBid = null;

                if (!string.IsNullOrEmpty(startingBidInput))
                {
                    startingBid = decimal.Parse(startingBidInput);
                }

                Console.Write("");
                Console.WriteLine("1. Hatchback");
                Console.WriteLine("2. Sedan");
                Console.WriteLine("3. SUV");
                Console.WriteLine("4. Truck");
                Console.Write("Vehicle Type (leave blank to skip): ");

                var vehicleTypeInput = Console.ReadLine();

                VehicleType? vehicleType = null;

                if (!string.IsNullOrEmpty(vehicleTypeInput))
                {
                    switch (vehicleTypeInput)
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
                }

                var results = this.auctionManager.SearchVehicles(manufacturer, model, year, startingBid, vehicleType);

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

                var auctions = this.auctionManager.GetAuctions();

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
                    auction.AssociatedVehicle.StartingBid,
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
            var vehicle = this.auctionManager.GetVehicleById(uniqueIdentifier);
            vehicle.DisplayDetails();
        }
    }
}
