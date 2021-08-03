using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Ioc;
using MediaManager;
using MediaManager.Library;
using MediaManager.Playback;
using MediaManager.Player;
using MediaManager.Queue;
using MusicApp.Services;
using MusicApp.ViewModel;
using Xamarin.Forms;
using PositionChangedEventArgs = MediaManager.Playback.PositionChangedEventArgs;

namespace MusicApp.Static
{
    public class MediaController : BaseViewModel
    {
        static readonly Lazy<MediaController> _instance = new Lazy<MediaController>();

        public static MediaController Instance => _instance.Value;

        public IMediaManager Media => CrossMediaManager.Current;

        public double _lastestPos;

        private IFileService _fileService;

        #region properties

        private bool _isHasPlayer;

        public bool IsHasPlayer
        {
            get => _isHasPlayer;
            set
            {
                _isHasPlayer = value;
                OnPropertyChanged();
            }
        }

        private bool _isPlaying;

        public bool IsPlaying
        {
            get => _isPlaying;
            set
            {
                _isPlaying = value;
                OnPropertyChanged();
            }
        }

        private double _progress;

        public double Progress
        {
            get => _progress;
            set
            {
                _progress = value;
                OnPropertyChanged();
            }
        }

        private string _currentSongName;

        public string CurrentSongName
        {
            get => _currentSongName;
            set
            {
                _currentSongName = value;
                OnPropertyChanged();
            }
        }

        private string _currentAuthorName;

        public string CurrentAuthorName
        {
            get => _currentAuthorName;
            set
            {
                _currentAuthorName = value;
                OnPropertyChanged();
            }
        }

        private string _currentSongThumbnail;

        public string CurrentSongThumbnail
        {
            get => _currentSongThumbnail;
            set
            {
                _currentSongThumbnail = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _currentTimeSpan;

        public TimeSpan CurrentTimeSpan
        {
            get => _currentTimeSpan;
            set
            {
                _currentTimeSpan = value;
                OnPropertyChanged();
            }
        }

        private TimeSpan _currentDuration;

        public TimeSpan CurrentDuration
        {
            get => _currentDuration;
            set
            {
                _currentDuration = value;
                OnPropertyChanged();
            }
        }

        private RepeatMode _repeatMode;

        public RepeatMode RepeatMode
        {
            get => _repeatMode;
            set
            {
                _repeatMode = value;
                OnPropertyChanged();
            }
        }

        private ShuffleMode _shuffleMode;

        public ShuffleMode ShuffleMode
        {
            get => _shuffleMode;
            set
            {
                _shuffleMode = value;
                OnPropertyChanged();
            }
        }

        public ICommand PlayToggleCommand => new Command(PlayToggle);
        public ICommand NextCommand => new Command(PlayNextSong);
        public ICommand PrevCommand => new Command(PlayPrevious);
        public ICommand RepeatCommand => new Command(ToggleRepeatMode);
        public ICommand ShuffleCommand => new Command(ToggleShuffle);

        #endregion properties

        public MediaController() : base()
        {
            _fileService = SimpleIoc.Default.GetInstance<IFileService>();

            Media.AutoPlay = true;
            Media.MediaItemChanged += Media_MediaItemChanged;
            Media.MediaItemFinished += Media_MediaItemFinished;
            Media.PositionChanged += Media_PositionChanged;

            Media.MediaItemFailed += (sender, args) =>
            {

            };
            
            Media.StateChanged += (sender, e) =>
            {
                Console.WriteLine("property: " + e.State);

                switch (e.State)
                {
                    case MediaPlayerState.Stopped:

                        IsPlaying = false;
                        IsHasPlayer = false;
                        Progress = 0;

                        break;

                    case MediaPlayerState.Playing:

                        IsPlaying = true;
                        IsHasPlayer = true;

                        break;

                    case MediaPlayerState.Paused:

                        IsPlaying = false;
                        IsHasPlayer = true;

                        break;
                }
            };

            Media.Notification.ShowNavigationControls = true;

            RepeatMode = Media.RepeatMode;
            ShuffleMode = Media.ShuffleMode;
        }

        public void RemovePosChangedHandler()
        {
         //   Media.PositionChanged -= Media_PositionChanged;
        }

        private void Media_MediaItemFinished(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            //Media.PositionChanged -= Media_PositionChanged;
            _lastestPos = 0;
            Progress = 0;
        }

        private void Media_MediaItemChanged(object sender, MediaManager.Media.MediaItemEventArgs e)
        {
            _lastestPos = 0;
            var media = e.MediaItem;

            Media.Queue.Current.DisplayImageUri = media.DisplayImageUri;
            Media.Queue.Current.AlbumImageUri = media.DisplayImageUri;
            Media.Queue.Current.ImageUri = media.DisplayImageUri;
            Media.Queue.Current.MediaUri = media.DisplayImageUri;

            Media.Notification.UpdateNotification();
            Media.Extractor.UpdateMediaItem(CrossMediaManager.Current.Queue.Current);

            CurrentSongName = media.Title;
            CurrentAuthorName = media.Artist;
            CurrentSongThumbnail = media.DisplayImageUri;
            CurrentDuration = Media.Queue.Current.Duration;

            //Media.PositionChanged -= Media_PositionChanged;
            Media.PositionChanged += Media_PositionChanged;
        }

        private void Media_PositionChanged(object sender, PositionChangedEventArgs e)
        {
            Console.WriteLine("Pos: "+ e.Position);
            var currentSeconds = e.Position.TotalSeconds;
            if (currentSeconds - _lastestPos < 1)
                return;

            _lastestPos = currentSeconds;

            var totalSecond = Media.Duration.TotalSeconds;

            var percent = (currentSeconds / totalSecond) * 100;

            CurrentTimeSpan = e.Position;

            Progress = percent;
        }

        #region methods

        void PlayToggle()
        {
            if (Media.IsPlaying())
            {
                this.Pause();
            }
            else
            {
                this.Play();
            }
        }

        public async void AddSongToPlayingQueue(SongItemViewModel song)
        {
            var media = await GetSong(song);

            if (media == null)
                return;

            if (Media.Queue.Count <= 0)
            {
                Play(media);
                return;
            }
            else
                Media.Queue.Add(media);
        }

        public void ToggleShuffle()
        {
            Media.ShuffleMode = Media.ShuffleMode == ShuffleMode.All ? ShuffleMode.Off : ShuffleMode.All;
            ShuffleMode = Media.ShuffleMode;
        }

        public void ToggleRepeatMode()
        {
            switch (RepeatMode)
            {
                case RepeatMode.All:
                    SetRepeatMode(RepeatMode.One);
                    return;
                case RepeatMode.Off:
                    SetRepeatMode(RepeatMode.All);
                    return;
                case RepeatMode.One:
                    SetRepeatMode(RepeatMode.Off);
                    return;
            }
        }

        public void SetRepeatMode(RepeatMode mode)
        {
            Media.RepeatMode = mode;
            RepeatMode = mode;
        }

        public void PlayPreviousOrSeekToStart()
        {
            Media.PlayPreviousOrSeekToStart();
        }

        public void Seek(double percent)
        {
            var val = Media.Duration.TotalSeconds * (percent / 100);

            Media.SeekTo(TimeSpan.FromSeconds(val));
        }

        public void PlayPrevious()
        {
            RemovePosChangedHandler();
            Media.PlayPrevious();
        }

        public void PlayNextSong()
        {
            RemovePosChangedHandler();
            Media.PlayNext();
        }

        public async void Play()
        {
            if (Media.Queue.HasCurrent)
            {
                Media.Play();
            }
        }

        public void Pause()
        {
            if (Media.Queue.HasCurrent)
            {
                Media.Pause();
            }
        }

        public void Stop()
        {
            if (Media.Queue.HasCurrent)
                Media.Stop();
        }

        public async void PlaySong(SongItemViewModel song)
        {
            var media = await GetSong(song);
            
            if (media == null)
                return;
            
            await Play(media);
        }

        async Task Play(IMediaItem media)
        {
           // Media.PositionChanged -= Media_PositionChanged;

            Media.Queue.Clear();

            if (Device.RuntimePlatform == Device.Android)
            {
                await Media.Play(media);
            }
            else
            {
                if(Media.Queue.Count <= 0)
                {
                    await Media.Play(media);
                    return;
                }
                Media.Queue.Add(media);

                await Media.PlayQueueItem(media);
            }
        }

        public async void PlayOnQueueById(string id)
        {
            var item = Media.Queue.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (item == null)
                return;

            if (item.Id.Equals(Media.Queue.Current.Id))
            {
                StaticUI.Instance.StartLoading("This song is playing");
                await Media.SeekToStart();
                return;
            }

            //Media.PositionChanged -= Media_PositionChanged;

            await Media.PlayQueueItem(item);
        }

        public bool RemoveItemFromQueue(string id)
        {
            var item = Media.Queue.Where(p => p.Id.Equals(id)).FirstOrDefault();

            if (item == null)
                return false;

            if (item.Id.Equals(Media.Queue.Current.Id))
            {
                StaticUI.Instance.StartLoading("Can't remove the playing song!");
                return false;
            }

            return Media.Queue.Remove(item);
        }

        public async void PlayAll(SafeObservableCollection<SongItemViewModel> downloadedSongs)
        {
            var firstSong = await GetSong(downloadedSongs[0]);

            if (firstSong == null)
                return;

            Media.Queue.Clear();

            await Play(firstSong);

            for (int i = 1; i < downloadedSongs.Count; i++)
            {
                var song = await GetSong(downloadedSongs[i]);

                Media.Queue.Add(song);
            }
        }

        public async Task<IMediaItem> GetSong(SongItemViewModel song)
        {
            var item = song.Type == SongTypes.Offline
           ? await Media.Extractor.CreateMediaItem(new System.IO.FileInfo(Path.Combine(_fileService.GetStorageFolderPath(), song.Url)))
           : await Media.Extractor.CreateMediaItem(song.Url);

            if (item != null)
            {
                item.Id = song.Id;
                item.IsMetadataExtracted = false;
                item.Title = song.Title;
                item.DisplayTitle = song.Title;
                item.DisplayDescription = song.Description;
                item.Artist = song.AuthorName;
                item.Extras = song.Type;

                item.DisplayImageUri = song.BigThumbnailUrl;

                return item;
            }
            else
            {
                StaticUI.Instance.ToastMesage("Can't load this song!, please try again later");

                return null;
            }
        }

        public SafeObservableCollection<SongItemViewModel> GetPlayingSongs(Action<string> onSelect)
        {
            var list = new SafeObservableCollection<SongItemViewModel>();

            if (Media.Queue.Count <= 0)
                return list;

            foreach (var mediItem in Media.Queue)
            {
                list.Add(new SongItemViewModel()
                {
                    Id = mediItem.Id,
                    Title = mediItem.Title,
                    AuthorName = mediItem.Artist,
                    Url = mediItem.MediaUri,
                    SmallThumbnailUrl = mediItem.DisplayImageUri,
                    BigThumbnailUrl = mediItem.DisplayImageUri,
                    Description = mediItem.DisplayDescription,
                    Type = (SongTypes)mediItem.Extras,
                    OnPlay = onSelect
                });
            }

            return list;
        }

        #endregion methods
    }
}