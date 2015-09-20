using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorDepot.WEB.Models
{
    public enum VoyageSortOrders
    {
        [Display(Name="Имени")]
        Name,

        [Display(Name="Статусу")]
        Status,

        [Display(Name="Времени начала")]
        StartTime,

        [Display(Name="Времени конца")]
        EndTime,

        [Display(Name="Времени выполнения")]
        LeadTime
    }
}