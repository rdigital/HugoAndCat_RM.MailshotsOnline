/*
     * Case insensitive string comparison.
     *
     * http://stackoverflow.com/questions/12822417/mvc3-compareattribute-for-case-insensitive-comparison
     */

using System;
using System.ComponentModel.DataAnnotations;
using System.Globalization;

namespace RM.MailshotsOnline.Entities.Attributes
{
    public class CompareCaseInsensitiveAttribute : CompareAttribute
    {

        public CompareCaseInsensitiveAttribute(string otherProperty) : base(otherProperty)
        {
        }

        protected override ValidationResult IsValid(object value, ValidationContext validationContext)
        {
            var property = validationContext.ObjectType.GetProperty(this.OtherProperty);
            if (property == null)
            {
                return new ValidationResult(string.Format(CultureInfo.CurrentCulture, "Unknown property {0}", this.OtherProperty));
            }

            var otherValue = property.GetValue(validationContext.ObjectInstance, null) as string;
            if (string.Equals(value as string, otherValue, StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            return new ValidationResult(this.FormatErrorMessage(validationContext.DisplayName));
        }
    }
}