using CarAuctionManagementSystem.Models;
using Newtonsoft.Json;

namespace CarAuctionManagementSystem.Services
{
    public class AuctionService
    {
        private List<IVehicle> vehicles;
        private string dataFilePath;

        public AuctionService(string dataFilePath)
        {
            if (string.IsNullOrEmpty(dataFilePath) || !Directory.Exists(Path.GetDirectoryName(dataFilePath)))
            {
                throw new ArgumentException("Invalid File Path");
            }

            this.dataFilePath = dataFilePath;
            LoadData();
        }

        private void LoadData()
        {
            if (File.Exists(dataFilePath))
            {
                string jsonData = File.ReadAllText(dataFilePath);
                vehicles = JsonConvert.DeserializeObject<List<IVehicle>>(jsonData);
            }
            else
            {
                vehicles = new List<IVehicle>();
            }
        }

        private void SaveData()
        {
            string jsonData = JsonConvert.SerializeObject(vehicles);
            File.WriteAllText(dataFilePath, jsonData);
        }

        public void AddVehicle(IVehicle vehicle)
        {
            if (vehicles.Any(v => v.UniqueIdentifier == vehicle.UniqueIdentifier))
            {
                throw new InvalidOperationException("A vehicle with the same unique identifier already exists.");
            }
            vehicles.Add(vehicle);
            SaveData();
        }

        public List<IVehicle> SearchVehicles(Func<IVehicle, bool> predicate)
        {
            return predicate is null ? vehicles.ToList() : vehicles.Where(predicate).ToList();
        }
    }
}
