using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;
using WpfScheduledApp20250729.Models;

namespace WpfScheduledApp20250729.Views
{
    public class FieldTypeToVisibilityConverter : IValueConverter
    {
        public static FieldTypeToVisibilityConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is FormFieldType fieldType && parameter is string targetTypeString)
            {
                var isMatch = fieldType.ToString() == targetTypeString;
                return isMatch ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }

    public class BooleanToVisibilityConverter : IValueConverter
    {
        public static BooleanToVisibilityConverter Instance = new();

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool boolValue)
            {
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}