using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class VehicleClassDTO : EntityBaseDTO
    {
        public String Name { get; set; }
        public String Description { get; set; }
    }
}
