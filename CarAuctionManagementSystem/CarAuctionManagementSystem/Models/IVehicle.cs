using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CarAuctionManagementSystem.Models
{
    public interface IVehicle
    {
        string UniqueIdentifier { get; }
        string Manufacturer { get; }
        string Model { get; }
        int Year { get; }
        decimal StartingBid { get; }
        void DisplayDetails(); // Método para exibir detalhes do veículo
    }
}