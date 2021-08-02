using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.ViewModel;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Pages
{
    [XamlCompilation(XamlCompilationOptions.Compile)]
    public partial class PlayingQueuePage : BasePage
    {
        public PlayingQueuePage()
        {
            this.BindingContext = SimpleIoc.Default.GetInstance<PlayingQueueViewModel>();
            InitializeComponent();
        }
    }
}
