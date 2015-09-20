using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace MotorDepot.DAL.Entities
{
    public class Driver : EntityBase
    {

        public String FirstName { get; set; }
        public String LastName { get; set; }

        public Int32 DriverLicenseId { get; set; }
        public DriverLicense DriverLicense { get; set; }

        public Gender Gender { get; set; }
        public DateTime BithDate { get; set; }

        public User User { get; set; }

        public ICollection<DriverVoyageRequest> Requests { get; set; }
        
    }

    public class DriverLicense
    {
        public Int32 Id { get; set; }
        public ICollection<VehicleClass> VehicleClasses { get; set; }
        public DateTime IssueDate { get; set; }
        public String IssuedBy { get; set; }

        public Driver Driver { get; set; }
    }

    public enum Gender
    {
        Male,
        Female
    }
}
