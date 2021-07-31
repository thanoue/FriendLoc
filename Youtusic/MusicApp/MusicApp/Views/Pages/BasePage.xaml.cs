using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MusicApp.Static;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class BasePage : ContentPage
    {
        #region Bindable Properties

        public static readonly BindableProperty ViewContentProperty = BindableProperty.Create
            (nameof(ViewContent), typeof(View), typeof(BasePage));

        public View ViewContent
        {
            get { return (View) GetValue(ViewContentProperty); }
            set { SetValue(ViewContentProperty, value); }
        }

        #endregion Bindable Properties

        public BasePage()
        {
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();
            
            MessagingCenter.Send<BasePage,object>(this,Constants.LAYOUT_APPEARED,this.BindingContext);
        }
    }
}