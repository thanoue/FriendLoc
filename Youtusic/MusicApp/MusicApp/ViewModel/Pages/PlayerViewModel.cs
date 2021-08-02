using MediaManager;
using MediaManager.Library;
using MusicApp.Model;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Text;
using System.Windows.Input;
using GalaSoft.MvvmLight.Views;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Static;
using Xamarin.Essentials;
using Xamarin.Forms;

namespace MusicApp.ViewModel
{
    public class PlayerViewModel : BasePageViewModel
    {
        public override string PageName => nameof(PlayerPage);
        public override bool IsModal => true;

        public override bool IsShowPlayer => false;

        public override ICommand BackCommand => new Command(()
            =>
            Navigation.DismissModal());

        public PlayerViewModel(ICustomNavigationService navigation, IDownloadService downloadService,
            ISecureStorageService secureStorageService, IFileService fileService, IApiClient apiClient)
            : base(navigation, downloadService, secureStorageService, fileService, apiClient)
        {

        }

        public ICommand ViewPlaylistCommand => new Command(ViewPlaylist);

        // public ICommand ShareCommand => new Command(() => Share.RequestAsync(selectedMusic.Url, selectedMusic.Title));

        void ViewPlaylist()
        {
            if (Navigation.CurrentPageKey != nameof(PlayingQueuePage))
                Navigation.NavigateTo(nameof(PlayingQueuePage));

            Navigation.DismissModal();
        }
    }
}