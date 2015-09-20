using System;
using System.Collections.Generic;
using System.Data.Entity.Validation;
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

        public void Append(DbEntityValidationException exception)
        {
            foreach (var validationError in exception.EntityValidationErrors.SelectMany(entityValidationError => entityValidationError.ValidationErrors))
            {
                Errors.Add(new PropertyMessagePair { PropertyName = validationError.PropertyName, Message = validationError.ErrorMessage });
            }
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
