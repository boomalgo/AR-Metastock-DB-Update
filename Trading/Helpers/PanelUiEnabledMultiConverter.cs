using System;
using System.Globalization;
using System.Linq;
using System.Windows;
using System.Windows.Data;

namespace Trading.Helpers
{
    public class PanelUiEnabledMultiConverter : IMultiValueConverter
    {
        public object Convert(object[] values, Type targetType, object parameter, CultureInfo culture)
        {
            if (values.Any(t => t == null || t == DependencyProperty.UnsetValue))
                return false;

            var result = bool.Parse(values[0].ToString());
            result = values.Aggregate(result, (current, value) => current && bool.Parse(value.ToString()));
            return result;
        }

        public object[] ConvertBack(object value, Type[] targetTypes, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
