using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfScheduledApp20250729.Views
{
    public class BoolToViewButtonTextConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isLibraryView)
            {
                return isLibraryView ? "📊 TABLE VIEW" : "📚 LIBRARY VIEW";
            }
            return "📚 LIBRARY VIEW";
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}