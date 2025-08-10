using System;
using System.Globalization;
using System.Windows.Data;

namespace WpfScheduledApp20250729.Views
{
    public class BooleanToRotationConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool isVisible)
            {
                return isVisible ? 0 : 180; // メニューが見える時は0度（右向き矢印）、隠れている時は180度（左向き矢印）
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}