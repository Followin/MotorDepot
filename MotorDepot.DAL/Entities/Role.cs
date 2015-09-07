using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class Role : EntityBase
    {
        public String Name { get; set; }
        public ICollection<User> Users { get; set; } 
    }
}
