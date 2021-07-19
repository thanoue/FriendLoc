using Xamarin.Forms;
using Xamarin.Forms.Internals;

namespace iCho.UI.Controls
{
    /// <summary>
    /// This class is inherited from Xamarin.Forms.Entry to remove the border for Entry control in the Android platform.
    /// </summary>
    [Preserve(AllMembers = true)]
    public class BorderlessEntry : Entry
    {
        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(Text), typeof(bool), typeof(BorderlessEntry));

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set
            {
                SetValue(IsValidProperty, value);
            }
        }
    }
}