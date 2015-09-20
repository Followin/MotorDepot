using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace MotorDepot.WEB.Models
{
    public class CreateVehicleViewModel
    {
        [Required]
        [Display(Name="Название")]
        public String Name { get; set; }

        [Required]
        [Display(Name="Тип")]
        public Int32 VehicleClassId { get; set; }

        [Required]
        [Display(Name="Кол-во сидений")]
        public Int32 SeatsNumber { get; set; }

        [Required]
        public DimensionsViewModel Dimensions { get; set; }

        [Required]
        public CreateDriveViewModel Drive { get; set; }

        [Required]
        [Display(Name="Фото")]
        public HttpPostedFileBase Photo { get; set; }
    }

    public class EditVehicleViewModel
    {
        public Int32 Id { get; set; }

        [Required]
        [Display(Name = "Название")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public Int32 VehicleClassId { get; set; }

        [Required]
        [Display(Name = "Кол-во сидений")]
        public Int32 SeatsNumber { get; set; }

        [Required]
        public DimensionsViewModel Dimensions { get; set; }
        [Required]
        public CreateDriveViewModel Drive { get; set; }

        [Display(Name = "Фото")]
        public HttpPostedFileBase Photo { get; set; }
    }

    public class VehicleViewModel
    {
        public Int32 Id { get; set; }


        [Required]
        [Display(Name = "Название")]
        public String Name { get; set; }

        [Required]
        [Display(Name = "Тип")]
        public VehicleClassViewModel Class { get; set; }

        [Required]
        [Display(Name = "Кол-во сидений")]
        public Int32 SeatsNumber { get; set; }

        [Required]
        public DimensionsViewModel Dimensions { get; set; }

        [Required]
        public DriveViewModel Drive { get; set; }

        [Required]
        [Display(Name = "Фото")]
        public ImageViewModel Photo { get; set; }
    }

    public class DimensionsViewModel
    {
        [Required]
        [Display(Name = "Длина(м)")]
        public Double Length { get; set; }

        [Required]
        [Display(Name="Ширина(м)")]
        public Double Width { get; set; }

        [Required]
        [Display(Name="Высота(м)")]
        public Double Height { get; set; }
    }

    public class CreateDriveViewModel
    {

        public Int32 Id { get; set; }

        [Required]
        [Display(Name="Название")]
        public String Name { get; set; }

        [Required]
        [Display(Name="Объем(л)")]
        public Double Volume { get; set; }

        [Required]
        [Display(Name="Тип привода")]
        public VehicleDriveType DriveType { get; set; }

        [Required]
        [Display(Name="Топливо")]
        public Int32 FuelTypeId { get; set; }

        [Required]
        [Display(Name="Кол-во цилиндров")]
        public Int32 CylindersNumber { get; set; }

        [Required]
        [Display(Name="Максимальная скорость(км/ч)")]
        public Int32 MaxSpeed { get; set; }

        [Required]
        [Display(Name="Разгон от 0 до 100(с)")]
        public Double AccelerationTime { get; set; }
    }

    public class DriveViewModel
    {
        [Display(Name = "Название")]
        public String Name { get; set; }

        [Display(Name = "Объем")]
        public Double Volume { get; set; }

        [Display(Name = "Тип привода")]
        public VehicleDriveType DriveType { get; set; }

        [Display(Name = "Топливо")]
        public FuelTypeViewModel FuelType { get; set; }

        [Display(Name = "Кол-во цилиндров")]
        public Int32 CylindersNumber { get; set; }

        [Display(Name = "Максимальная скорость")]
        public Int32 MaxSpeed { get; set; }

        [Display(Name = "Разгон от 0 до 100")]
        public Double AccelerationTime { get; set; }
    }


    public class VehicleClassViewModel
    {

        public Int32 Id { get; set; }
        [Display(Name="Тип авто")]
        public String Name { get; set; }
        public String Description { get; set; }
    }

    public class FuelTypeViewModel
    {
        public Int32 Id { get; set; }
        public String Name { get; set; }
    }

    public enum VehicleDriveType
    {
        [Display(Name="Задний привод")]
        RearWheel,

        [Display(Name = "Полный привод")]
        FourWheel,

        [Display(Name = "Передний привод")]
        FrontWheel
    }



}