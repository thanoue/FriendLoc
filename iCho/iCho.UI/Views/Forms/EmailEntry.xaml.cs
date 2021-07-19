using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace iCho.UI.Views.Forms
{
    /// <summary>
    /// View used to show the email entry with validation status.
    /// </summary>
    [Preserve(AllMembers = true)]
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class EmailEntry
    {
        public static readonly BindableProperty TextProperty = BindableProperty.Create(nameof(Text), typeof(string), typeof(EmailEntry));

        public string Text
        {
            get => (string)GetValue(TextProperty);
            set
            {
                SetValue(TextProperty, value);
            }
        }

        public static readonly BindableProperty IsValidProperty = BindableProperty.Create(nameof(Text), typeof(bool), typeof(EmailEntry));

        public bool IsValid
        {
            get => (bool)GetValue(IsValidProperty);
            set
            {
                SetValue(IsValidProperty, value);
            }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="EmailEntry" /> class.
        /// </summary>
        public EmailEntry()
        {
            InitializeComponent();
        }
    }
}