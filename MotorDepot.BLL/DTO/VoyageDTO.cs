using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class VoyageDTO : EntityBaseDTO
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public VoyageStatus Status { get; set; }
        public VoyagePointDTO StartPoint { get; set; }
        public VoyagePointDTO EndPoint { get; set; }
        public DateTime RequestedStartTime { get; set; }
        public DateTime RequestedEndTime { get; set; }

        public Int32? DriverId { get; set; }
        public DriverDTO Driver { get; set; }

        public Int32 VehicleId { get; set; }
        public VehicleDTO Vehicle { get; set; }

        public VoyageLifeCycleDTO LifeCycle { get; set; }
    }

    public enum VoyageStatus
    {
        Open,
        Accepted,
        Processing,
        Succeded,
        Canceled
    }

    public class VoyageLifeCycleDTO
    {
        public DateTime? Opened { get; set; }
        public DateTime? Acceped { get; set; }
        public DateTime? ProcessingStart { get; set; }
        public DateTime? Succeded { get; set; }
        public DateTime? Canceled { get; set; }
    }

    public class VoyagePointDTO
    {
        public String Name { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
    
}
