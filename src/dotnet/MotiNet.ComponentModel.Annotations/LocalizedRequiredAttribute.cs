using System.ComponentModel.DataAnnotations.Resources;

namespace System.ComponentModel.DataAnnotations
{
    public class LocalizedRequiredAttribute : RequiredAttribute
    {
        public LocalizedRequiredAttribute()
        {
            ErrorMessageResourceName = nameof(RequiredAttribute).Replace("Attribute", string.Empty);
            ErrorMessageResourceType = typeof(CommonValidationMessages);
        }
    }
}
