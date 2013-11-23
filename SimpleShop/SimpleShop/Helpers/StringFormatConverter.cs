#region Using Statements

using System;
using System.Globalization;
using System.Windows.Data;

#endregion

namespace SimpleShop.Helpers
{
    internal class StringFormatConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            return String.Format(values[1].ToString(), values[0]);
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}