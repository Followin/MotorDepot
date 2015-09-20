using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using MotorDepot.WEB.Filters;

namespace MotorDepot.WEB.Models
{
    public class VoyageViewModel
    {
        public Int32 Id { get; set; }

        [Display(Name = "Название")]
        public String Name { get; set; }

        [Display(Name = "Описание")]
        public String Description { get; set; }

        [Display(Name="Статус")]
        public VoyageStatus Status { get; set; }

        [Display(Name="Точка отправления")]
        public VoyagePointViewModel StartPoint { get; set; }

        [Display(Name="Конечная точка")]
        public VoyagePointViewModel EndPoint { get; set; }

        [Display(Name="Время начала")]
        public DateTime RequestedStartTime { get; set; }

        [Display(Name="Предположительное время окончания")]
        public DateTime RequestedEndTime { get; set; }

        [Display(Name="Водитель")]
        public RegisterDriverViewModel Driver { get; set; }

        [Display(Name="Машина")]
        public VehicleViewModel Vehicle { get; set; }
        public VoyageLifeCycleViewModel LifeCycle { get; set; }
    }

    public class CreateVoyageViewModel
    {
        [Required]
        [Display(Name="Название")]
        public String Name { get; set; }
        
        [Required]
        [Display(Name="Описание")]
        public String Description { get; set; }

        [Required]
        public VoyagePointViewModel StartPoint { get; set; }

        [Required]
        public VoyagePointViewModel EndPoint { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name="Дата начала")]
        public DateTime RequestedStartDate { get; set; }

        public Int32 VehicleId { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name="Время начала")]
        public DateTime RequestedStartTime { get; set; }

        [Required]
        [DataType(DataType.Date)]
        [Display(Name = "Предположительная дата конца")]
        public DateTime RequestedEndDate { get; set; }

        [Required]
        [DataType(DataType.Time)]
        [Display(Name = "Предположительное время конца")]
        public DateTime RequestedEndTime { get; set; }
    }

    public enum VoyageStatus
    {
        [Display(Name="Открыт")]
        Open,

        [Display(Name="Утвержден")]
        Accepted,

        [Display(Name="Выполняется")]
        Processing,

        [Display(Name="Выполнен")]
        Succeded,

        [Display(Name="Отменен")]
        Canceled
    }

    public class VoyageLifeCycleViewModel
    {
        public DateTime? Opened { get; set; }
        public DateTime? Acceped { get; set; }
        public DateTime? ProcessingStart { get; set; }
        public DateTime? Succeded { get; set; }
        public DateTime? Canceled { get; set; }
    }

    public class VoyagePointViewModel
    {
        [Required]
        [Display(Name="Адрес")]
        public String Name { get; set; }
        public Double Latitude { get; set; }
        public Double Longitude { get; set; }
    }
}