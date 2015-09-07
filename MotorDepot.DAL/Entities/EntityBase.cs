using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.DAL.Entities
{
    public class EntityBase
    {
        public Int32 Id { get; set; }
        public EntryState EntryState { get; set; }

        public EntityBase()
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
