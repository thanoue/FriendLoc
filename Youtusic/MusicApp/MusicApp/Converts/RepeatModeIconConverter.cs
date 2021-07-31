using System;
using System.Globalization;
using System.Linq;
using MediaManager.Playback;
using Plugin.Iconize;
using Xamarin.Forms;

namespace MusicApp.Converts
{
    public class RepeatModeIconConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            try
            {
                var mode = (RepeatMode) value;
                var icon = "";
                switch (mode)
                {
                    case RepeatMode.All:
                        icon = "md-repeat";
                        break; 
                    case RepeatMode.Off:
                        icon = "md-repeat";
                        break;
                    case RepeatMode.One:
                        icon = "md-repeat-one";
                        break;
                }
                
                var module = Iconize.Modules.FirstOrDefault(p => p.FontName == "MaterialIcons-Regular");

                if (module == null)
                    return "";

                var val = module.GetIcon(icon);

                return val.Character;
            }
            catch (Exception e)
            {
                Console.WriteLine(e);

                return "";
            }   
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}