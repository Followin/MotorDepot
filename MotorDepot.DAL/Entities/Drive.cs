using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class Drive : EntityBase
    {
        public String Name { get; set; }
        public Double Volume { get; set; }
        public VehicleDriveType DriveType { get; set; }
        public FuelType Fuel { get; set; }
        public Int32 CylindersNumber { get; set; }
        public Int32 MaxSpeed { get; set; }
        public Double AccelerationTime { get; set; }


    }

    public enum VehicleDriveType
    {
        RearWheel,
        FourWheel,
        FrontWheel
    }

    public class FuelType : EntityBase
    {
        public String Name { get; set; }
    }
}
