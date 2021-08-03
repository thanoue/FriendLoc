using System;
using System.Collections.Generic;
using GalaSoft.MvvmLight.Ioc;
using MusicApp.Static;
using MusicApp.ViewModel;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MusicApp.Pages
{
    public class UrlModel
    {
        public string Id { get; set; }
        public IdTypes Type { get; set; }
        public enum IdTypes
        {
            Video,
            Playlist,
            Channel
        }
    }

    public partial class HomePage
    {
        string _url = "";
        UrlModel _id ;

        public HomePage(string url = "")
        {
            _url = url;

            this.BindingContext = SimpleIoc.Default.GetInstance<HomeViewModel>();

            InitializeComponent();

            if (!string.IsNullOrEmpty(_url))
            {
                _id = Utils.GetId(url);
            }
            else
            {
                //GetUrlFromClipBoard();
            }
        }

        async void GetUrlFromClipBoard()
        {
            var hasText = Clipboard.HasText;

            if (!hasText)
                return;

            var text = await Clipboard.GetTextAsync();

            if(!string.IsNullOrEmpty(text) && text.Contains("youtu"))
            {
                _id = Utils.GetId(text);
                Clipboard.SetTextAsync("");
            }
        }

       
        protected override  void OnAppearing()
        {
            base.OnAppearing();

            if (_id != null)
            {
                MessagingCenter.Send<object, UrlModel>(this, Constants.GET_SONG_FROM_YOUTUBE, _id);
                _id = null;
            }
        }
    }
}