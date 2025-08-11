using System;
using System.Collections.Generic;
using WpfScheduledApp20250729.Validation;

namespace WpfScheduledApp20250729.Models
{
    public enum FormFieldType
    {
        Text,
        Number,
        Date,
        Time,
        Boolean,
        TextArea,
        ComboBox
    }

    internal class FormFieldModel : NotificationObject
    {
        private object? _value;
        private string _errorMessage = string.Empty;

        public string PropertyName { get; set; } = string.Empty;
        public string DisplayName { get; set; } = string.Empty;
        public FormFieldType FieldType { get; set; }
        public bool IsRequired { get; set; }
        public bool IsReadOnly { get; set; }
        public List<ValidationRule> ValidationRules { get; set; } = new();
        public List<object>? ComboBoxItems { get; set; }

        public object? Value
        {
            get => _value;
            set
            {
                if (SetProperty(ref _value, value))
                {
                    ValidateField();
                }
            }
        }

        public string ErrorMessage
        {
            get => _errorMessage;
            set => SetProperty(ref _errorMessage, value);
        }

        public bool HasError => !string.IsNullOrEmpty(ErrorMessage);

        private void ValidateField()
        {
            ErrorMessage = string.Empty;

            foreach (var rule in ValidationRules)
            {
                var result = rule.Validate(Value ?? string.Empty, DisplayName);
                if (!result.IsValid)
                {
                    ErrorMessage = result.ErrorMessage;
                    break;
                }
            }
        }

        public ValidationResult Validate()
        {
            ValidateField();
            return HasError ? ValidationResult.Error(ErrorMessage) : ValidationResult.Success();
        }
    }
}