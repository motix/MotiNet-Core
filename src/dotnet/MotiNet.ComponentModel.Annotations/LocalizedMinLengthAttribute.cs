using System.ComponentModel.DataAnnotations.Resources;

namespace System.ComponentModel.DataAnnotations
{
    public class LocalizedMinLengthAttribute : MinLengthAttribute
    {
        public LocalizedMinLengthAttribute(int length) : base(length)
        {
            ErrorMessageResourceName = nameof(MinLengthAttribute).Replace("Attribute", string.Empty);
            ErrorMessageResourceType = typeof(CommonValidationMessages);
        }
    }
}
