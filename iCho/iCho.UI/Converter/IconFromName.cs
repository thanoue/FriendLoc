using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Plugin.Iconize;
using Xamarin.Forms;

namespace iCho.UI.Converter
{
    public abstract class IconConverter : BindableObject,IValueConverter
    {
        public static readonly BindableProperty IconProperty = BindableProperty.Create(nameof(Icon), typeof(string), typeof(MaterialIcon));
        protected abstract string ModuleName { get; }

        public string Icon
        {
            get { return (string)GetValue(IconProperty); }
            set { SetValue(IconProperty, value); }
        }

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var module = Iconize.Modules.FirstOrDefault(p => p.FontName == ModuleName);

                if (module == null)
                    return Icon;

                var icon = module.GetIcon(Icon);

                return icon.Character;
            }
            catch
            {
                return Icon;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class MaterialIcon : IconConverter, IValueConverter
    {
        protected override string ModuleName => "MaterialIcons-Regular";

        public MaterialIcon()
        {

        }
    }

    public class MaterialDesignIcon : IconConverter, IValueConverter
    {
        protected override string ModuleName => "Material-Design-Icons";

        public MaterialDesignIcon()
        {

        }
    }
}
