using System;
using System.ComponentModel.DataAnnotations;

namespace HotelManagementMVC.ValidationAttributes
{
    public class DateGreaterThanAttribute : ValidationAttribute
    {
        private readonly string _comparisonProperty;

        public DateGreaterThanAttribute(string comparisonProperty)
        {
            _comparisonProperty = comparisonProperty;
        }

        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
            if (value == null) return ValidationResult.Success;

            var currentValue = (DateTime)value;
            
            var property = validationContext.ObjectType.GetProperty(_comparisonProperty);
            if (property == null)
            {
                return new ValidationResult($"Property {_comparisonProperty} not found.");
            }

            var comparisonValue = (DateTime?)property.GetValue(validationContext.ObjectInstance);

            if (comparisonValue != null && currentValue <= comparisonValue)
            {
                return new ValidationResult(ErrorMessage ?? $"Date must be after {_comparisonProperty}.");
            }

            return ValidationResult.Success;
        }
    }
    
    public class DateInFutureAttribute : ValidationAttribute
    {
        protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
        {
             if (value == null) return ValidationResult.Success;
             
             var date = (DateTime)value;
             if (date.Date < DateTime.Today)
             {
                 return new ValidationResult("Date cannot be in the past.");
             }
             return ValidationResult.Success;
        }
    }
}
