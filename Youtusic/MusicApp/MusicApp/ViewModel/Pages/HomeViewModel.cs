using System.Collections.Generic;
using System.Linq;
using System.Windows.Input;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Services.Impl;
using MusicApp.Static;
using MusicApp.Views.Popups;
using Xamarin.Essentials;
using Xamarin.Forms;
using static MusicApp.Pages.UrlModel;

namespace MusicApp.ViewModel
{
    public class HomeViewModel : BasePageViewModel
    {
        #region Properties

        private SafeObservableCollection<SongItemViewModel> _downloadedSongs;

        public SafeObservableCollection<SongItemViewModel> DownloadedSongs
        {
            get => _downloadedSongs;
            set
            {
                _downloadedSongs = value;
                OnPropertyChanged(nameof(DownloadedSongs));
            }
        }

        public ICommand GoToSearchCommand => new Command(async () => { Navigation.NavigateTo(nameof(SearchPage)); });
        public ICommand PlayAllCommand => new Command(PlayAll);

        public override string PageName => nameof(HomePage);

        #endregion

        public HomeViewModel(ICustomNavigationService navigation,
            IDownloadService downloadService,
            ISecureStorageService secureStorageService,
            IFileService fileService, IApiClient apiClient) :
            base(navigation, downloadService, secureStorageService, fileService, apiClient)
        {
          
        }

        void PlayAll()
        {
            if (DownloadedSongs == null || !DownloadedSongs.Any())
                return;

            MediaController.Instance.PlayAll(DownloadedSongs);
        }

        protected override async void OnLayouAppeared()
        {
            base.OnLayouAppeared();

            DownloadedSongs = SecureStorageService.GetDownloadedSongs(OnSongSelected);

        }

        void OnSongSelected(string id)
        {
            var song = DownloadedSongs.FirstOrDefault(p => p.Id.Equals(id));

            var menuItems = new List<BottomMenuItem>()
            {
                new BottomMenuItem()
                {
                    Icon = "play_arrow",
                    Title = "Play Now",
                    Value = 0
                },
                new BottomMenuItem()
                {
                    Icon = "playlist_add",
                    Title = "Add To Queue",
                    Value = 1
                },
                new BottomMenuItem()
                {
                    Icon = "remove_circle",
                    Title = "Delete",
                    Value = 2
                }
            };

            var dialog = new MenuPopup((item) =>
            {
                switch ((int)item.Value)
                {
                    case 0:

                        PlaySong(song);

                        break;

                    case 1:

                        AddSongToPlayingQueue(song);

                        break;

                    case 2:

                        RemoveSong(song);

                        break;
                }
            }, menuItems);
            dialog.Show();
        }

        private void AddSongToPlayingQueue(SongItemViewModel song)
        {
            MediaController.Instance.AddSongToPlayingQueue(song);
        }

        private void PlaySong(SongItemViewModel song)
        {
            MediaController.Instance.PlaySong(song);
        }


        private async void RemoveSong(SongItemViewModel song)
        {
            //var confirm = await MaterialDialog.Instance.ConfirmAsync(
            //    message: "Do you want to delete this song forever?",
            //    confirmingText: "Delete",
            //    dismissiveText: "Cancel");

            //if (confirm == true)
            //{
            //    SecureStorageService.DeleteSongFromStorage(FileService, song);
            //    DownloadedSongs.Remove(song);
            //}
        }
    }
}