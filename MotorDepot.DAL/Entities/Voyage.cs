using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class Voyage : EntityBase
    {
        public String Name { get; set; }
        public String Description { get; set; }
        public VoyageStatus Status { get; set; }
        public VoyagePoint StartPoint { get; set; }
        public VoyagePoint EndPoint { get; set; }
        public Driver Driver { get; set; }
        public Vehicle Vehicle { get; set; }
        public VoyageLifeCycle LifeCycle { get; set; }
    }

    public enum VoyageStatus
    {
        Open,
        Accepted,
        Processing,
        Succeded,
        Canceled
    }

    public class VoyageLifeCycle
    {
        public DateTime? Opened { get; set; }
        public DateTime? Acceped { get; set; }
        public DateTime? ProcessingStart { get; set; }
        public DateTime? Succeded { get; set; }
        public DateTime? Canceled { get; set; }
    }

    public class VoyagePoint
    {
        public String Name { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}
