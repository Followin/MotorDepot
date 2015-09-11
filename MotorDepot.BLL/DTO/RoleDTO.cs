using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MotorDepot.DAL.Entities;

namespace MotorDepot.BLL.DTO
{
    public class RoleDTO
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
        public ICollection<User> Users { get; set; } 
    }
}
