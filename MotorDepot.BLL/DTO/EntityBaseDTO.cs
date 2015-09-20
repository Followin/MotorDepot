using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.BLL.DTO
{
    public class EntityBaseDTO
    {

        public Int32 Id { get; set; }
        public EntryState EntryState { get; set; }

        public EntityBaseDTO()
        {
            EntryState = EntryState.Active;
        }
    }

    public enum EntryState
    {
        Active,
        Inactive,
        Removed
    }
}
