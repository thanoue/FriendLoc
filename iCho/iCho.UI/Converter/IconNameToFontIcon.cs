using System;
using System.Globalization;
using Plugin.Iconize;
using Xamarin.Forms;

namespace iCho.UI.Converter
{
    public class IconNameToFontIcon : BindableObject, IValueConverter
    {
        public static readonly BindableProperty ConvParamProperty = BindableProperty.Create(nameof(ConvParam), typeof(string), typeof(IconNameToFontIcon));

        public string ConvParam
        {
            get { return (string)GetValue(ConvParamProperty); }
            set { SetValue(ConvParamProperty, value); }
        }

        public IconNameToFontIcon()
        {
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            //var val = (string)value;

            if (string.IsNullOrEmpty(ConvParam))
                return new IconButton();

            var icon = new IconImage()
            {
                Icon = ConvParam,
                IconColor = Color.Black,
                HeightRequest = 40,
                IconSize = 25,
                WidthRequest = 40,
                BackgroundColor=Color.Transparent
            };

            return icon;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return null;
        }
    }
}
