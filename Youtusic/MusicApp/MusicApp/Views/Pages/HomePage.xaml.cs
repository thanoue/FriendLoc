using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.ViewModel;
using Xamarin.Forms;

namespace MusicApp.Pages
{
    public partial class HomePage 
    {
        public HomePage()
        {
            this.BindingContext = SimpleIoc.Default.GetInstance<HomeViewModel>();

            InitializeComponent();
        }
    }
}