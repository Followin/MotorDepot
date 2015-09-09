using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class DriverVoyageRequest
    {
        public Int32 Id { get; set; }
        public Driver Driver { get; set; }
        public Voyage Voyage { get; set; }
        //public VehiclePreferences VehiclePreferences { get; set; }

    }

    //public class VehiclePreferences
    //{
    //    public Int32 Id { get; set; }
    //    public VehicleClass Class { get; set; }
    //    public Int32? MinSeatsNumber { get; set; }
    //    public Double? MinLength { get; set; }
    //    public Double? MinWidth { get; set; }
    //    public Double? MinHeight { get; set; }
    //    public VehicleDriveType DriveType { get; set; }
    //    public Int32? RequiredSpeed { get; set; }
    //    public Int32? MinVolume { get; set; }
    //    public FuelType FuelType { get; set; }

    //}
}
