using FFImageLoading.Forms;
using MusicApp.Static;
using Xamarin.Forms;

namespace MusicApp.Controls
{
    public class CustomImg : CachedImage
    {
        public static readonly BindableProperty SrcProperty = BindableProperty.Create(nameof(Src), typeof(string), typeof(CustomImg), "", propertyChanged: OnChange);
        public string Src
        {
            get => (string)GetValue(SrcProperty);
            set => SetValue(SrcProperty, value);
        }

        private static void OnChange(BindableObject bindable, object oldValue, object newValue)
        {
            var control = (CustomImg)bindable;

            if (newValue == null)
            {
                control.Source = null;
                return;
            }

            if (newValue is string)
            {
                var res = newValue as string;

                if(string.IsNullOrEmpty(res))
                {
                    control.Source = null;
                    return;
                }

                var uri = res.GetUriFromString();

                if (uri == null)
                    control.Source = ((string)newValue).GetImageFromResource();
                else
                    control.Source = ImageSource.FromUri(uri);

                return;
            }

        }

    }
}