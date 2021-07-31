using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Plugin.Iconize;
using Xamarin.Forms;

namespace MusicApp.Converter
{
    public class MediaStateConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value == null)
                return "";
            
            var isPlay = (bool) value;
            
            try
            {
                var module = Iconize.Modules.FirstOrDefault(p => p.FontName == "MaterialIcons-Regular");

                if (module == null)
                    return "";

                var icon = module.GetIcon(isPlay ? "md-pause-circle-outline" :"md-play-circle-outline" );

                return icon.Character;
            }
            catch
            {
                return "";
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return false;
        }
    }
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

                return LoadIcon(module);
            }
            catch
            {
                return Icon;
            }
        }

        protected virtual char  LoadIcon(IIconModule module)
        {
            var icon = module.GetIcon(Icon);

            return icon.Character;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return value;
        }
    }

    public class MaterialIcon : IconConverter
    {
        protected override string ModuleName => "MaterialIcons-Regular";

        protected override char LoadIcon(IIconModule module)
        {
            var raw = "md-" + Icon.Replace("_", "-");

            return module.GetIcon(raw).Character;
        }

        public MaterialIcon()
        {

        }
    }

    public class MaterialDesignIcon : IconConverter
    {
        protected override string ModuleName => "Material-Design-Icons";

        public MaterialDesignIcon()
        {

        }
    }
}
