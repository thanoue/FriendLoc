using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.Static;
using MusicApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayerPage 
    {
        public PlayerPage()
        {
            this.BindingContext = SimpleIoc.Default.GetInstance<PlayerViewModel>();
            InitializeComponent();
        }

        protected override void OnAppearing()
        {
            base.OnAppearing();

            RotateImageContinously();
        }
        
        async Task RotateImageContinously()
        {
            while (true) 
            {
                for (int i = 1; i < 7; i++)
                {
                    if (image.Rotation >= 360f) image.Rotation = 0;
                    await image.RotateTo(i * (360 / 6), 1000, Easing.Linear);
                }
            }
        }

        private void Slider_OnDragCompleted(object sender, EventArgs e)
        {
            var val = (Slider) sender;
            
            MediaController.Instance.Seek(val.Value);
        }
    }
}