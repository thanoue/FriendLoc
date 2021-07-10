using Xamarin.Forms;
using Xamarin.Forms.Internals;
using Xamarin.Forms.Xaml;

namespace iCho.UI.Views.Forms
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    [Preserve(AllMembers = true)]
    public partial class TabbedForm : ContentPage
    {
        public TabbedForm()
        {
            InitializeComponent();
        }
    }
}