using CarAuctionManagementSystem.Models;

namespace CarAuctionManagementSystem.Domain
{
    public class AuctionManager
    {
        private Dictionary<string, Auction> auctions;
        private List<IVehicle> vehicles;

        public AuctionManager()
        {
            auctions = new Dictionary<string, Auction>();
            vehicles = new List<IVehicle>();
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
                    int hatchbackDoors = Convert.ToInt32(additionalParameters["NumDoors"]);
                    newVehicle = new Hatchback(uniqueIdentifier, manufacturer, model, year, startingBid, hatchbackDoors);
                    break;
                case VehicleType.Sedan:
                    int sedanDoors = Convert.ToInt32(additionalParameters["NumDoors"]);
                    newVehicle = new Sedan(uniqueIdentifier, manufacturer, model, year, startingBid, sedanDoors);
                    break;
                case VehicleType.SUV:
                    int suvSeats = Convert.ToInt32(additionalParameters["NumSeats"]);
                    newVehicle = new SUV(uniqueIdentifier, manufacturer, model, year, startingBid, suvSeats);
                    break;
                case VehicleType.Truck:
                    int truckLoadCapacity = Convert.ToInt32(additionalParameters["LoadCapacity"]);
                    newVehicle = new Truck(uniqueIdentifier, manufacturer, model, year, startingBid, truckLoadCapacity);
                    break;
                default:
                    throw new ArgumentException("Invalid vehicle type.");
            }

            vehicles.Add(newVehicle);
        }


        public List<IVehicle> SearchVehicles(string manufacturer = null, string model = null, int? year = null, decimal? startingBid = null, VehicleType? vehicleType = null)
        {
            var result = new List<IVehicle>();

            foreach (var vehicle in vehicles)
            {
                // Verifica se o tipo de veículo corresponde ao tipo especificado na pesquisa
                if (IsVehicleTypeMatch(vehicle, vehicleType) &&
                    (String.IsNullOrEmpty(manufacturer) || vehicle.Manufacturer == manufacturer) &&
                    (String.IsNullOrEmpty(model) || vehicle.Model == model) &&
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

            var vehicle = this.GetVehicleById(vehicleId);
            if (vehicle == null)
            {
                throw new InvalidOperationException("Vehicle not found.");
            }

            if (auctions.ContainsKey(vehicleId))
            {
                if (auctions[vehicleId].IsActive)
                {
                    throw new InvalidOperationException("Auction is already active for this vehicle.");
                }

                auctions[vehicleId].Start();
            }
            else
            {
                var auction = new Auction(vehicle, vehicle.StartingBid);
                auction.Start();
                auctions.Add(vehicleId, auction);
            }
        }

        public void CloseAuction(string vehicleId)
        {
            ValidateVehicleId(vehicleId);

            if (auctions.ContainsKey(vehicleId))
            {
                if (auctions[vehicleId].IsActive)
                {
                    auctions[vehicleId].Close();
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
            if (auctions.ContainsKey(vehicleId))
            {
                auctions[vehicleId].PlaceBid(amount, bidderName);
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public Auction GetAuction(string vehicleId)
        {
            if (auctions.ContainsKey(vehicleId))
            {
                return auctions[vehicleId];
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public List<Auction> GetAuctions()
        {
            return auctions.Select(x => x.Value).ToList();
        }

        public decimal GetHighestBid(string vehicleId)
        {
            if (auctions.ContainsKey(vehicleId))
            {
                return auctions[vehicleId].CurrentHighestBid;
            }
            else
            {
                throw new InvalidOperationException("No active auction found for this vehicle.");
            }
        }

        public IVehicle GetVehicleById(string uniqueIdentifier) => vehicles.FirstOrDefault(v => v.UniqueIdentifier == uniqueIdentifier);

        private bool IsVehicleTypeMatch(IVehicle vehicle, VehicleType? vehicleType)
        {
            if (!vehicleType.HasValue)
            {
                return true;
            }

            switch (vehicleType)
            {
                case VehicleType.Hatchback:
                    return vehicle is Hatchback;
                case VehicleType.Sedan:
                    return vehicle is Sedan;
                case VehicleType.SUV:
                    return vehicle is SUV;
                case VehicleType.Truck:
                    return vehicle is Truck;
                default:
                    return false;
            }
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
