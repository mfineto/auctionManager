namespace CarAuctionManagementSystem.Domain
{
    using CarAuctionManagementSystem.Models;

    public class AuctionManager
    {
        private readonly Dictionary<string, Auction> auctions;
        private readonly List<IVehicle> vehicles;

        public AuctionManager()
        {
            this.auctions = new Dictionary<string, Auction>();
            this.vehicles = new List<IVehicle>();
        }

        public void AddVehicle(string uniqueIdentifier, string manufacturer, string model, int year, decimal startingBid, VehicleType vehicleType, Dictionary<string, object> additionalParameters)
        {
            IVehicle newVehicle;

            if (this.GetVehicleById(uniqueIdentifier) is not null)
            {
                throw new ArgumentException("A vehicle with the same unique identifier already exists.");
            }

            switch (vehicleType)
            {
                case VehicleType.Hatchback:
                    var hatchbackDoors = Convert.ToInt32(additionalParameters["NumDoors"]);
                    newVehicle = new Hatchback(uniqueIdentifier, manufacturer, model, year, startingBid, hatchbackDoors);
                    break;
                case VehicleType.Sedan:
                    var sedanDoors = Convert.ToInt32(additionalParameters["NumDoors"]);
                    newVehicle = new Sedan(uniqueIdentifier, manufacturer, model, year, startingBid, sedanDoors);
                    break;
                case VehicleType.SUV:
                    var suvSeats = Convert.ToInt32(additionalParameters["NumSeats"]);
                    newVehicle = new SUV(uniqueIdentifier, manufacturer, model, year, startingBid, suvSeats);
                    break;
                case VehicleType.Truck:
                    var truckLoadCapacity = Convert.ToInt32(additionalParameters["LoadCapacity"]);
                    newVehicle = new Truck(uniqueIdentifier, manufacturer, model, year, startingBid, truckLoadCapacity);
                    break;
                default:
                    throw new ArgumentException("Invalid vehicle type.");
            }

            this.vehicles.Add(newVehicle);
        }

        public List<IVehicle> SearchVehicles(string manufacturer = null, string model = null, int? year = null, decimal? startingBid = null, VehicleType? vehicleType = null)
        {
            var result = new List<IVehicle>();

            foreach (var vehicle in this.vehicles)
            {
                if (IsVehicleTypeMatch(vehicle, vehicleType) &&
                    (string.IsNullOrEmpty(manufacturer) || vehicle.Manufacturer == manufacturer) &&
                    (string.IsNullOrEmpty(model) || vehicle.Model == model) &&
                    (!year.HasValue || vehicle.Year == year) &&
                    (!startingBid.HasValue || vehicle.StartingBid == startingBid))
                {
                    result.Add(vehicle);
                }
            }

            return result;
        }

        public void StartAuction(string vehicleId)
        {
            ValidateVehicleId(vehicleId);

            var vehicle = this.GetVehicleById(vehicleId) ?? throw new InvalidOperationException("Vehicle not found.");

            if (this.auctions.TryGetValue(vehicleId, out var value))
            {
                if (value.IsActive)
                {
                    throw new InvalidOperationException("Auction is already active for this vehicle.");
                }

                value.Start();
            }
            else
            {
                var auction = new Auction(vehicle, vehicle.StartingBid);
                auction.Start();
                this.auctions.Add(vehicleId, auction);
            }
        }

        public void CloseAuction(string vehicleId)
        {
            ValidateVehicleId(vehicleId);

            if (this.auctions.TryGetValue(vehicleId, out var value))
            {
                if (value.IsActive)
                {
                    value.Close();
                }
                else
                {
                    throw new InvalidOperationException("Auction is not active for this vehicle.");
                }
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public void PlaceBid(string vehicleId, decimal amount, string bidderName)
        {
            if (this.auctions.TryGetValue(vehicleId, out var value))
            {
                value.PlaceBid(amount, bidderName);
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public Auction GetAuction(string vehicleId)
        {
            if (this.auctions.ContainsKey(vehicleId))
            {
                return this.auctions[vehicleId];
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public List<Auction> GetAuctions()
        {
            return this.auctions.Select(x => x.Value).ToList();
        }

        public decimal GetHighestBid(string vehicleId)
        {
            if (this.auctions.TryGetValue(vehicleId, out var value))
            {
                return value.CurrentHighestBid;
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public IVehicle GetVehicleById(string uniqueIdentifier) => this.vehicles.FirstOrDefault(v => v.UniqueIdentifier == uniqueIdentifier);

        private static bool IsVehicleTypeMatch(IVehicle vehicle, VehicleType? vehicleType)
        {
            if (!vehicleType.HasValue)
            {
                return true;
            }

            return vehicleType switch
            {
                VehicleType.Hatchback => vehicle is Hatchback,
                VehicleType.Sedan => vehicle is Sedan,
                VehicleType.SUV => vehicle is SUV,
                VehicleType.Truck => vehicle is Truck,
                _ => false,
            };
        }

        private static void ValidateVehicleId(string vehicleId)
        {
            if (string.IsNullOrEmpty(vehicleId))
            {
                throw new ArgumentNullException(nameof(vehicleId), "Vehicle ID cannot be null or empty.");
            }
        }
    }
}
