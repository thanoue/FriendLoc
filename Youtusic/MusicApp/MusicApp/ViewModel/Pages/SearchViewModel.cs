using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Command;
using GalaSoft.MvvmLight.Views;
using MusicApp.Model.ApiModels;
using MusicApp.Services;
using MusicApp.Static;
using MusicApp.Views.Popups;
using Newtonsoft.Json;
using Plugin.MaterialDesignControls;
using Xamarin.Forms;

namespace MusicApp.ViewModel
{
    public class SearchViewModel : BasePageViewModel
    {
        #region Fields

        private int _pageItemCount = 20;
        public ICommand SearchReturnCommand { get; }
        public ICommand SearchNextPageCommand { get; }
        public ICommand SearchPrevPageCommand { get; }

        #endregion

        #region Properties

        private string _searchTerm;
        public string SearchTerm
        {
            get => _searchTerm;
            set
            {
                _searchTerm = value;
                OnPropertyChanged(nameof(SearchTerm));
            }
        }

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

        private SafeObservableCollection<SongItemViewModel> _searchedSongs;
        public SafeObservableCollection<SongItemViewModel> SearchedSongs
        {
            get => _searchedSongs;
            set
            {
                _searchedSongs = value;
                OnPropertyChanged(nameof(SearchedSongs));
            }
        }

        #endregion

        public SearchViewModel(ICustomNavigationService navigation,IDownloadService downloadService, IApiClient apiClient, ISecureStorageService secureStorageService,IFileService fileService) 
            : base(navigation,downloadService, secureStorageService,fileService,apiClient)
        {
            NextPageToken = "";
            PrevPageToken = "";
            SearchedSongs = new SafeObservableCollection<SongItemViewModel>();
            SearchReturnCommand = new RelayCommand(() => { OnSearch(""); });
            SearchNextPageCommand = new RelayCommand(OnNextPage);
            SearchPrevPageCommand = new RelayCommand(OnPrevPage);

            SearchTerm = "yeu em dai lau";
        }

        #region Methods
        void MenuItemClicked(string id)
        {
            var song = SearchedSongs.FirstOrDefault(p => p.Id.Equals(id));

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
            if (SecureStorageService.SongIsDownloaded(song.Id))
            {
                StaticUI.Instance.ToastMesage("This song has been already downloaded.");
                return;
            }

            song = await GetSongUrl(song);

            if(song != null)
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
                    SearchedSongs.SafeClear();
                    SearchedSongs.ObtainFromSelecting(medias.RelatedVideos, MenuItemClicked);
                    SecureStorageService.CombineSongs(SearchedSongs);
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

        async void OnSearch(string pageToken)
        {
            StaticUI.Instance.StartLoading();

            var model = new SearchApiModel()
            {
                MaxResults = _pageItemCount,
                Terms = SearchTerm,
                PageToken = pageToken
            };
            var res = await ApiClient.SearchVideos(model);

            if (res != null)
            {
                StaticUI.Instance.StopLoading();
                PrevPageToken = res.PageInfo.PrevPageToken;
                NextPageToken = res.PageInfo.NextPageToken;
                SearchedSongs.SafeClear();
                SearchedSongs.ObtainFromSearching(res.Results, MenuItemClicked);
                SecureStorageService.CombineSongs(SearchedSongs);
            }
            else
            {
                StaticUI.Instance.StopLoading();
            }
        }

        #endregion

    }
}