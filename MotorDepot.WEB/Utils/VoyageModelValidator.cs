using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MotorDepot.WEB.Models;

namespace MotorDepot.WEB.Utils
{
    public class VoyageModelValidator : ModelValidator
    {
        public VoyageModelValidator(ModelMetadata metadata, ControllerContext controllerContext) : base(metadata, controllerContext)
        {
        }

        public override IEnumerable<ModelValidationResult> Validate(object container)
        {
            var result = new Collection<ModelValidationResult>();
            var voyage = Metadata.Model as CreateVoyageViewModel;

            var requestedStartTime =
                voyage.RequestedStartDate.AddHours(voyage.RequestedStartTime.Hour)
                    .AddMinutes(voyage.RequestedStartTime.Minute);
            var requestedEndTime =
                voyage.RequestedEndDate.AddHours(voyage.RequestedEndTime.Hour)
                    .AddMinutes(voyage.RequestedEndTime.Minute);


            if(requestedStartTime < DateTime.Now)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Невозможно создать рейс, начинающийся ранее текущего времени"});

            if(requestedEndTime < DateTime.Now)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Невозможно создать рейс, заканчивающийся ранее текущего времени"});

            if(requestedEndTime <= requestedStartTime)
                result.Add(new ModelValidationResult {MemberName = "", Message = "Дата окончания должна быть больше, чем дата начала"});

            return result;

        }
    }
}