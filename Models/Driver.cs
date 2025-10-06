using System.ComponentModel.DataAnnotations;
using System.Diagnostics;

namespace TruckScheduler.Models
{
    public class Driver
    {
        public int Id { get; set; }
        [Required] public string FullName { get; set; }
        public string LicenseNumber { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public ICollection<Truck> Trucks { get; set; }
    }
}
