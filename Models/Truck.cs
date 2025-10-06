using System.ComponentModel.DataAnnotations;

namespace TruckScheduler.Models
{
    public class Truck
    {
        public int Id { get; set; }
        [Required] public string LicensePlate { get; set; }
        public string Model { get; set; }
        public string Manufacturer { get; set; }

        public int? DriverId { get; set; }
        public Driver Driver { get; set; }

        public ICollection<CheckIn> CheckIns { get; set; }
    }
}
