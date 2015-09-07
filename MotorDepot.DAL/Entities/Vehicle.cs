using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class Vehicle : EntityBase
    {
        public String Name { get; set; }
        public VehicleClass Class { get; set; }
        public Int32 SeatsNumber { get; set; }
        public Dimensions Dimensions { get; set; }
        public Drive Drive { get; set; }
    }

    public class Dimensions
    {
        public Int32 Id { get; set; }
        public Double Length { get; set; }
        public Double Width { get; set; }
        public Double Height { get; set; }
    }

    
}
