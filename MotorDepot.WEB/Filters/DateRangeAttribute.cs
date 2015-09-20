using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorDepot.WEB.Filters
{
    public class DateRangeAttribute : ValidationAttribute
    {
        private DateTime minTime;
        private DateTime maxTime;

        public DateRangeAttribute(DateTime minDate, DateTime maxDate)
        {
            minTime = minDate;
            maxTime = maxDate;
        }
    }
}