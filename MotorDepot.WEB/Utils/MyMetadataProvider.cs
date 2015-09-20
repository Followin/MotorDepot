using System;
using System.ComponentModel;
using System.Web;
using System.Web.Mvc;

namespace MotorDepot.WEB.Utils
{
    public class MyMetadataProvider : DataAnnotationsModelMetadataProvider
    {
        protected override ModelMetadata GetMetadataForProperty(Func<object> modelAccessor, Type containerType, PropertyDescriptor propertyDescriptor)
        {
            var result = base.GetMetadataForProperty(modelAccessor, containerType, propertyDescriptor);
            if (result.TemplateHint == null)
                if (typeof (Enum).IsAssignableFrom(result.ModelType))
                {
                    result.TemplateHint = "Enum";
                }
                else if (typeof (HttpPostedFileBase).IsAssignableFrom(result.ModelType))
                {
                    result.TemplateHint = "File";
                }
            return result;
        }
    }
}