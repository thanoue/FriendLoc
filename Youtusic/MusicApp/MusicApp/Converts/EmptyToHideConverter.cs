using System;
using System.Globalization;
using Xamarin.Forms;

namespace MusicApp.Converts
{
    public class EmptyToHideConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var val = value.ToString();
                
                return !String.IsNullOrEmpty(val);
            }
            catch
            {
                return false;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return "";
        }
    }
}