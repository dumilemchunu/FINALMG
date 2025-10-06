using System.Diagnostics;
using TruckScheduler.Models;

namespace TruckScheduler.Data
{
    public static class DbInitializer
    {
        public static void Seed(ApplicationDbContext context)
        {
            if (context.ParkingBays.Any()) return;

            // seed parking bays (BayNumber, AllowedCargoType)
            var bays = new List<ParkingBay>
            {
                new ParkingBay { BayNumber = "A1", AllowedCargoType = CargoType.Hazardous, Notes = "Hazmat" },
                new ParkingBay { BayNumber = "A2", AllowedCargoType = CargoType.Hazardous, Notes = "Hazmat" },
                new ParkingBay { BayNumber = "B1", AllowedCargoType = CargoType.Refrigerated, Notes = "Refrigerated goods" },
                new ParkingBay { BayNumber = "B2", AllowedCargoType = CargoType.Refrigerated, Notes = "Refrigerated goods" },
                new ParkingBay { BayNumber = "C1", AllowedCargoType = CargoType.Perishable, Notes = "Perishable goods" },
                new ParkingBay { BayNumber = "D1", AllowedCargoType = CargoType.Fragile, Notes = "Fragile items" },
                new ParkingBay { BayNumber = "E1", AllowedCargoType = CargoType.Oversized, Notes = "Oversized trucks" },
                new ParkingBay { BayNumber = "G1", AllowedCargoType = CargoType.General, Notes = "General use" },
                new ParkingBay { BayNumber = "G2", AllowedCargoType = CargoType.General, Notes = "General use" }
            };
            context.ParkingBays.AddRange(bays);

            // sample drivers
            var drivers = new List<Driver>
            {
                new Driver { FullName = "John Mbeki", LicenseNumber = "DL12345", Phone = "0720001111", Email = "john@example.com" },
                new Driver { FullName = "Sibongile Ndlovu", LicenseNumber = "DL98765", Phone = "0720002222", Email = "sibongile@example.com" }
            };
            context.Drivers.AddRange(drivers);

            // sample trucks
            var trucks = new List<Truck>
            {
                new Truck { LicensePlate = "ABC123GP", Model = "Volvo FH", Manufacturer = "Volvo" },
                new Truck { LicensePlate = "XYZ987CA", Model = "Mercedes Actros", Manufacturer = "Mercedes" }
            };
            context.Trucks.AddRange(trucks);

            context.SaveChanges();
        }
    }
}
