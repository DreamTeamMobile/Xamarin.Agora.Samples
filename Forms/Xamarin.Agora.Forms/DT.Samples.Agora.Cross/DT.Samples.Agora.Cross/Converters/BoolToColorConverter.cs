using System;
using System.Globalization;
using Xamarin.Forms;

namespace DT.Samples.Agora.Cross.Converters
{
    public class BoolToColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var activeColot = parameter == null ? Color.Red : Color.FromHex(parameter.ToString());
            return (bool)value ? activeColot : Color.Silver;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}

