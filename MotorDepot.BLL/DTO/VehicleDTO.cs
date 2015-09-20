using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class VehicleDTO : EntityBaseDTO
    {
        public String Name { get; set; }
        
        public Int32 SeatsNumber { get; set; }
        public DimensionsDTO Dimensions { get; set; }

        public Int32 VehicleClassId { get; set; }
        public VehicleClassDTO Class { get; set; }

        public Int32 DriveId { get; set; }
        public DriveDTO Drive { get; set; }

        public ImageDTO Photo { get; set; }
    }

    public class DimensionsDTO
    {
        public Double Length { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
    }
}
