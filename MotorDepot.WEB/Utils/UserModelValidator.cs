using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Utils
{
    public class UserModelValidator : ModelValidator
    {
        public UserModelValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var result = new Collection<ModelValidationResult>();
            var user = Metadata.Model as RegisterUserViewModel;
            
            if(user.Driver.BithDate >= DateTime.Now)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Дата рождения не может быть менее текущей"});

            if(user.Driver.DriverLicense.IssueDate >= DateTime.Now)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Дата получения прав не может быть менее текущей"});

            if(user.Driver.DriverLicense.IssueDate <= user.Driver.BithDate)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Дата получения прав не может быть менее даты рождения"});

            return result;
        }
    }
}