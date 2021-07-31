using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Rg.Plugins.Popup.Pages;
using Rg.Plugins.Popup.Services;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Views.Popups
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class LoadingPopup : BasePopup
    {
        private string _message;
        public LoadingPopup(string message)
        {
            _message = message;
            InitializeComponent();
        }
        
   
    }
}