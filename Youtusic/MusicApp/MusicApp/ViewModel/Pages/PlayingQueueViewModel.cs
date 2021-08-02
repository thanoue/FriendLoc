using System.Collections.Generic;
using System.Linq;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Static;
using MusicApp.Views.Popups;

namespace MusicApp.ViewModel
{
    public class PlayingQueueViewModel : BasePageViewModel
    {
        public override string PageName => nameof(PlayingQueuePage);

        public override bool IsShowPlayer => true;

        #region Properties

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

        #endregion
        
        public PlayingQueueViewModel(ICustomNavigationService navigation, IDownloadService downloadService, ISecureStorageService secureStorageService, IFileService fileService,IApiClient apiClient) 
            : base(navigation, downloadService, secureStorageService, fileService,apiClient)
        {
            
        }

        protected override void OnLayouAppeared()
        {
            base.OnLayouAppeared();

            Songs = MediaController.Instance.GetPlayingSongs(OnSongSelected);
        }
        
        void OnSongSelected(string id)
        {
            var song = Songs.FirstOrDefault(p => p.Id.Equals(id));

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
                    Icon = "file_download",
                    Title = "Download",
                    Value = 1
                },
                new BottomMenuItem()
                {
                    Icon = "remove_circle",
                    Title = "Remove From Playlist",
                    Value = 2
                }
            };

            var dialog = new MenuPopup((item) =>
            {
                switch ((int) item.Value)
                {
                    case 0:

                        PlaySong(song);

                        break;

                    case 1:

                        Download(song);

                        break;

                    case 2:

                        RemoveFromPlaylist(song);

                        break;
                }
            }, menuItems);
            dialog.Show();
        }

        private  async  void Download(SongItemViewModel song)
        {
            if (song.Type == SongTypes.Offline)
            {
                StaticUI.Instance.ToastMesage("This song has been already downloaded.");
                return;
            }
            
            song = await GetSongUrl(song);

            if(song != null)
                DownloadUrl(song);
        }

        private void RemoveFromPlaylist(SongItemViewModel song)
        {
            if (MediaController.Instance.RemoveItemFromQueue(song.Id))
                Songs.Remove(song);
        }

        private void PlaySong(SongItemViewModel song)
        {
           MediaController.Instance.PlayOnQueueById(song.Id);
        }
    }
}