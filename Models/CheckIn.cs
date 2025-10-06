using System.ComponentModel.DataAnnotations;

namespace TruckScheduler.Models
{
    public class CheckIn
    {
        public int Id { get; set; }

        [Required] public int TruckId { get; set; }
        public Truck Truck { get; set; }

        public int? DriverId { get; set; }
        public Driver Driver { get; set; }

        public CargoType CargoType { get; set; }
        public string CargoDescription { get; set; }

        public DateTime CheckInTime { get; set; } = DateTime.UtcNow;
        public DateTime? CheckOutTime { get; set; }

        public int? ParkingBayId { get; set; }
        public ParkingBay ParkingBay { get; set; }

        public bool IsActive => CheckOutTime == null;
    }
}
