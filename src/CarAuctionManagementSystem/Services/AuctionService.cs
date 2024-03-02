namespace CarAuctionManagementSystem.Services
{
    using CarAuctionManagementSystem.Models;
    using Newtonsoft.Json;

    public class AuctionService
    {
        private readonly string dataFilePath;
        private List<IVehicle> vehicles;

        public AuctionService(string dataFilePath)
        {
            if (string.IsNullOrEmpty(dataFilePath) || !Directory.Exists(Path.GetDirectoryName(dataFilePath)))
            {
                throw new ArgumentException("Invalid File Path");
            }

            this.dataFilePath = dataFilePath;

            this.LoadData();
        }

        public void AddVehicle(IVehicle vehicle)
        {
            if (this.vehicles.Any(v => v.UniqueIdentifier == vehicle.UniqueIdentifier))
            {
                throw new InvalidOperationException("A vehicle with the same unique identifier already exists.");
            }

            this.vehicles.Add(vehicle);

            this.SaveData();
        }

        public List<IVehicle> SearchVehicles(Func<IVehicle, bool> predicate)
        {
            return predicate is null ? this.vehicles.ToList() : this.vehicles.Where(predicate).ToList();
        }

        private void LoadData()
        {
            if (File.Exists(this.dataFilePath))
            {
                var jsonData = File.ReadAllText(this.dataFilePath);
                this.vehicles = JsonConvert.DeserializeObject<List<IVehicle>>(jsonData);
            }
            else
            {
                this.vehicles = new List<IVehicle>();
            }
        }

        private void SaveData()
        {
            var jsonData = JsonConvert.SerializeObject(this.vehicles);
            File.WriteAllText(this.dataFilePath, jsonData);
        }
    }
}
