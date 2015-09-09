using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MotorDepot.BLL.Models
{
    public class ServiceResult
    {
        public ServiceResultStatus Status
        {
            get
            {
                return Errors.Count == 0
                    ? ServiceResultStatus.Success
                    : ServiceResultStatus.Error;
            }
        }

        public ICollection<PropertyMessagePair> Errors { get; set; }

        public ServiceResult()
        {
            Errors = new List<PropertyMessagePair>();
        }
    }

    public enum ServiceResultStatus
    {
        Success,
        Error
    }

    public struct PropertyMessagePair
    {
        public String PropertyName { get; set; }
        public String Message { get; set; }
    }
}
