using System.Collections.Generic;
using System.Linq;

namespace WpfScheduledApp20250729.Validation
{
    internal class ValidationManager
    {
        private readonly Dictionary<string, List<ValidationRule>> _rules = new();

        public void AddRule(string propertyName, ValidationRule rule)
        {
            if (!_rules.ContainsKey(propertyName))
            {
                _rules[propertyName] = new List<ValidationRule>();
            }
            _rules[propertyName].Add(rule);
        }

        public ValidationResult ValidateProperty(string propertyName, object value)
        {
            if (!_rules.ContainsKey(propertyName))
            {
                return ValidationResult.Success();
            }

            foreach (var rule in _rules[propertyName])
            {
                var result = rule.Validate(value, propertyName);
                if (!result.IsValid)
                {
                    return result;
                }
            }

            return ValidationResult.Success();
        }

        public Dictionary<string, string> ValidateAll(Dictionary<string, object> values)
        {
            var errors = new Dictionary<string, string>();

            foreach (var kvp in values)
            {
                var result = ValidateProperty(kvp.Key, kvp.Value);
                if (!result.IsValid)
                {
                    errors[kvp.Key] = result.ErrorMessage;
                }
            }

            return errors;
        }

        public bool IsValid(Dictionary<string, object> values)
        {
            return !ValidateAll(values).Any();
        }
    }
}