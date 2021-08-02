using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MusicApp.Model.ApiModels;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Static;
using MusicApp.Views.Popups;
using Newtonsoft.Json;
using Xamarin.Forms;

namespace MusicApp.ViewModel
{
    public abstract class BaseSearchViewModel : BasePageViewModel
    {
        #region Fields

        protected int PageItemCount = 20;
        public ICommand SearchNextPageCommand { get; }
        public ICommand SearchPrevPageCommand { get; }

        #endregion

        #region Properties

        private string _prevPageToken = "";
        public string PrevPageToken
        {
            get => _prevPageToken;
            set
            {
                _prevPageToken = value;
                OnPropertyChanged(nameof(PrevPageToken));
            }
        }

        private string _nextPageToken = "";
        public string NextPageToken
        {
            get => _nextPageToken;
            set
            {
                _nextPageToken = value;
                OnPropertyChanged(nameof(NextPageToken));
            }
        }

        private SafeObservableCollection<SongItemViewModel> _songs;
        public SafeObservableCollection<SongItemViewModel> Songs
        {
            get => _songs;
            set
            {
                _songs = value;
                OnPropertyChanged(nameof(Songs));
            }
        }

        public override string PageName => nameof(SearchPage);

        #endregion

        public BaseSearchViewModel(ICustomNavigationService navigation,
            IDownloadService downloadService,
            IApiClient apiClient,
            ISecureStorageService secureStorageService,
            IFileService fileService)
            : base(navigation, downloadService, secureStorageService, fileService, apiClient)
        {
            NextPageToken = "";
            PrevPageToken = "";
            Songs = new SafeObservableCollection<SongItemViewModel>();
            SearchPrevPageCommand = new RelayCommand(OnPrevPage);
            SearchNextPageCommand = new RelayCommand(OnNextPage);
        }

        #region Methods
        protected void MenuItemClicked(string id)
        {
            var song = Songs.FirstOrDefault(p => p.Id.Equals(id));

            var menuItems = new List<BottomMenuItem>()
            {
                new BottomMenuItem()
                {
                    Icon = "check",
                    Title = "Get Relatived Songs",
                    Value = 0
                },
                new BottomMenuItem()
                {
                    Icon = "play_arrow",
                    Title = "Play Now",
                    Value = 1
                },
                new BottomMenuItem()
                {
                    Icon = "playlist_add",
                    Title = "Add To Queue",
                    Value = 2
                },
                new BottomMenuItem()
                {
                    Icon = "file_download",
                    Title = "Download",
                    Value = 3
                }
            };

            var dialog = new MenuPopup((item) =>
            {
                switch ((int)item.Value)
                {
                    case 0:

                        SelectSong(id);

                        break;

                    case 1:

                        PlaySong(song);

                        break;

                    case 2:

                        AddSongToPlayingQueue(song);

                        break;

                    case 3:

                        DownloadSong(song);

                        break;
                }

            }, menuItems);
            dialog.Show();

        }

        async void AddSongToPlayingQueue(SongItemViewModel song)
        {
            if (song.Type == SongTypes.Offline)
            {
                MediaController.Instance.AddSongToPlayingQueue(song);
                return;
            }

            song = await GetSongUrl(song);

            if (song != null)
                MediaController.Instance.AddSongToPlayingQueue(song);
        }

        async void PlaySong(SongItemViewModel song)
        {
            if (song.Type == SongTypes.Offline)
            {
                MediaController.Instance.PlaySong(song);
                return;
            }

            song = await GetSongUrl(song);

            if (song != null)
                MediaController.Instance.PlaySong(song);
        }

        async void DownloadSong(SongItemViewModel song)
        {
            if (SecureStorageService.SongIsDownloaded(song.Id) != null)
            {
                StaticUI.Instance.ToastMesage("This song has been already downloaded.");
                return;
            }

            song = await GetSongUrl(song);

            if (song != null)
                DownloadUrl(song);
        }

        async void SelectSong(string id)
        {
            StaticUI.Instance.StartLoading();

            var medias = await ApiClient.GetMediaItems(id);

            if (medias != null)
            {
                try
                {
                    StaticUI.Instance.StopLoading();
                    NextPageToken = "";
                    PrevPageToken = "";
                    Songs.SafeClear();
                    Songs.ObtainFromSelecting(medias.RelatedVideos, MenuItemClicked);
                    SecureStorageService.CombineSongs(Songs);
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }
            }
            else
            {
                StaticUI.Instance.StopLoading();
            }
        }

        void OnNextPage()
        {
            if (string.IsNullOrEmpty(_nextPageToken))
                return;

            OnSearch(_nextPageToken);
        }

        void OnPrevPage()
        {
            if (string.IsNullOrEmpty(_prevPageToken))
                return;

            OnSearch(_prevPageToken);
        }

        public abstract void OnSearch(string pageToken);
      

        #endregion

    }
}