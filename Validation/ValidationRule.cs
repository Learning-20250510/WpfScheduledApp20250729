using System;

namespace WpfScheduledApp20250729.Validation
{
    internal abstract class ValidationRule
    {
        public abstract ValidationResult Validate(object value, string propertyName);
    }

    internal class ValidationResult
    {
        public bool IsValid { get; set; }
        public string ErrorMessage { get; set; } = string.Empty;

        public static ValidationResult Success() => new ValidationResult { IsValid = true };
        public static ValidationResult Error(string message) => new ValidationResult { IsValid = false, ErrorMessage = message };
    }
}