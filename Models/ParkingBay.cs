using System.ComponentModel.DataAnnotations;

namespace TruckScheduler.Models
{
    public class ParkingBay
    {
        public int Id { get; set; }
        [Required] public string BayNumber { get; set; }
        public CargoType AllowedCargoType { get; set; }
        public string Notes { get; set; }
    }
}
