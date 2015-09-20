using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace MotorDepot.WEB.Models
{
    public class RegisterUserViewModel
    {
        [Required]
        [Display(Name="Имя пользователя")]
        [Remote("IsNicknameFree", "Account", ErrorMessage = "Это имя пользователя занято")]
        public String Nickname { get; set; }

        [Required]
        [Display(Name="Пароль")]
        [DataType(DataType.Password)]
        public String Password { get; set; }

        [Required]
        [System.ComponentModel.DataAnnotations.Compare("Password", ErrorMessage = "Пароль и подтверждение должны совпадать")]
        [Display(Name="Подтверждение пароля")]
        [DataType(DataType.Password)]
        public String ConfirmPassword { get; set; }

        [Required]
        [DataType(DataType.EmailAddress)]
        [Display(Name="Email адрес")]
        public String Email { get; set; }
        

        public RegisterDriverViewModel Driver { get; set; }
    }

    public class UserViewModel
    {
        public Int32 Id { get; set; }

        [Display(Name = "Имя пользователя")]
        public String Nickname { get; set; }

        [DataType(DataType.EmailAddress)]
        [Display(Name = "Email адрес")]
        public String Email { get; set; }

        [Display(Name="Верифицирован")]
        public Boolean IsConfirmed { get; set; }

        public RoleViewModel Role { get; set; }

        public DriverViewModel Driver { get; set; }
    }

    public class LoginViewModel
    {
        [Required]
        [Display(Name="Имя пользователя")]
        public String Login { get; set; }

        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Пароль")]
        public String Password { get; set; }

        [Display(Name="Запомнить меня")]
        public Boolean Persistent { get; set; }
    }

    public class RegisterDriverViewModel
    {

        [Required]
        [Display(Name="Имя")]
        public String FirstName { get; set; }

        [Required]
        [Display(Name="Фамилия")]
        public String LastName { get; set; }

        [Required]
        [Display(Name="Пол")]
        public Gender Gender { get; set; }

        [Required]
        [Display(Name="Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BithDate { get; set; }

        public RegisterDriverLicenseViewModel DriverLicense { get; set; }
    }

    public class DriverViewModel
    {
        public Int32 Id { get; set; }
        [Display(Name = "Имя")]
        public String FirstName { get; set; }

        [Display(Name = "Фамилия")]
        public String LastName { get; set; }

        [Display(Name = "Пол")]
        public Gender Gender { get; set; }

        [Display(Name = "Дата рождения")]
        [DataType(DataType.Date)]
        public DateTime BithDate { get; set; }

        public DriverLicenseViewModel DriverLicense { get; set; }
    }

    public class DriverLicenseViewModel
    {
        [Display(Name = "Классы автомобилей")]
        public ICollection<VehicleClassViewModel> VehicleClasses { get; set; }

        [Display(Name = "Когда выданы")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Display(Name = "Кем выданы")]
        public String IssuedBy { get; set; }
    }

    public class RegisterDriverLicenseViewModel
    {
        [Required]
        [Display(Name="Классы автомобилей")]
        public Int32[] VehicleClasses { get; set; }

        [Required]
        [Display(Name="Когда выданы")]
        [DataType(DataType.Date)]
        public DateTime IssueDate { get; set; }

        [Required]
        [Display(Name="Кем выданы")]
        public String IssuedBy { get; set; }

    }

    public class RoleViewModel
    {
        public Int32 Id { get; set; }

        [Display(Name="Роль")]
        public String Name { get; set; }
    }
    

    public enum Gender
    {
        [Display(Name="Мужской")]
        Male,
        
        [Display(Name="Женский")]
        Female
    }
}