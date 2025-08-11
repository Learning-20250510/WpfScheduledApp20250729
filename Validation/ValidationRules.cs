using System;
using System.Text.RegularExpressions;

namespace WpfScheduledApp20250729.Validation
{
    internal class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, string propertyName)
        {
            if (value == null || string.IsNullOrWhiteSpace(value.ToString()))
            {
                return ValidationResult.Error($"{propertyName} is required.");
            }
            return ValidationResult.Success();
        }
    }

    internal class MaxLengthValidationRule : ValidationRule
    {
        private readonly int _maxLength;

        public MaxLengthValidationRule(int maxLength)
        {
            _maxLength = maxLength;
        }

        public override ValidationResult Validate(object value, string propertyName)
        {
            if (value?.ToString()?.Length > _maxLength)
            {
                return ValidationResult.Error($"{propertyName} must not exceed {_maxLength} characters.");
            }
            return ValidationResult.Success();
        }
    }

    internal class MinValueValidationRule : ValidationRule
    {
        private readonly int _minValue;

        public MinValueValidationRule(int minValue)
        {
            _minValue = minValue;
        }

        public override ValidationResult Validate(object value, string propertyName)
        {
            if (value is int intValue && intValue < _minValue)
            {
                return ValidationResult.Error($"{propertyName} must be at least {_minValue}.");
            }
            return ValidationResult.Success();
        }
    }

    internal class EmailValidationRule : ValidationRule
    {
        private readonly Regex _emailRegex = new Regex(@"^[^@\s]+@[^@\s]+\.[^@\s]+$", RegexOptions.IgnoreCase);

        public override ValidationResult Validate(object value, string propertyName)
        {
            var stringValue = value?.ToString();
            if (!string.IsNullOrWhiteSpace(stringValue) && !_emailRegex.IsMatch(stringValue))
            {
                return ValidationResult.Error($"{propertyName} must be a valid email address.");
            }
            return ValidationResult.Success();
        }
    }
}