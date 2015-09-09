using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class VehicleDTO : EntityBase
    {
        public String Name { get; set; }
        public VehicleClassDTO Class { get; set; }
        public Int32 SeatsNumber { get; set; }
        public DimensionsDTO Dimensions { get; set; }
        public Drive Drive { get; set; }
    }

    public class DimensionsDTO
    {
        public Double Length { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
    }
}
