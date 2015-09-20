using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Utils
{
    public class MyValidationProvider : ModelValidatorProvider
    {
        

        public override IEnumerable<ModelValidator> GetValidators(ModelMetadata metadata, ControllerContext context)
        {
            if (metadata.ModelType == typeof(CreateVoyageViewModel))
            {
                return new [] { new VoyageModelValidator(metadata, context)};
            }
            if (metadata.ModelType == typeof (RegisterUserViewModel))
            {
                return new[] {new UserModelValidator(metadata, context)};
            }
            return Enumerable.Empty<ModelValidator>();
        }
    }
}