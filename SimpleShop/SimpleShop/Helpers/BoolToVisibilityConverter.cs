#region Using Statements

using System.Windows;
using System.Windows.Data;

#endregion

namespace SimpleShop.Helpers
{
    /// <summary>
    /// Value converter class for converting boolean values to System.Windows.Visibility.
    /// </summary>
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if ((bool)value == true)
                return Visibility.Visible;
            else
                return Visibility.Hidden;
        }

        public object ConvertBack(object value, System.Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            return ((Visibility)value == Visibility.Hidden);
        }
    }
}
