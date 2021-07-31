using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Input;
using GalaSoft.MvvmLight.Views;
using MusicApp.Converter;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Static;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.PancakeView;

namespace MusicApp.ViewModel
{
    public abstract class BasePageViewModel : BaseViewModel
    {
        #region Base Field

        protected IDownloadService DownloadService;
        protected ISecureStorageService SecureStorageService;
        protected ICustomNavigationService Navigation;
        protected IFileService FileService;
        protected IApiClient ApiClient;
        public virtual bool IsShowPlayer => true;

        #endregion

        #region Download

        private double _progressValue;
        private Queue<SongItemViewModel> _downloadQueue;

        public double ProgressValue
        {
            get { return _progressValue; }
            set
            {
                _progressValue = value;
                OnPropertyChanged();
            }
        }

        private string _downloadingSongName;

        public string DownloadingSongName
        {
            get { return _downloadingSongName; }
            set
            {
                _downloadingSongName = value;
                OnPropertyChanged();
            }
        }

        private bool _isDownloading;

        public bool IsDownloading
        {
            get { return _isDownloading; }
            set
            {
                _isDownloading = value;
                OnPropertyChanged();
            }
        }


        #endregion

        public virtual ICommand BackCommand => new Command(() => Application.Current.MainPage.Navigation.PopAsync());

        public ICommand OpenPlayerCommand => new Command(OpenPlayer);

        public BasePageViewModel(ICustomNavigationService navigation,
            IDownloadService downloadService,
            ISecureStorageService secureStorageService,
            IFileService fileService,IApiClient apiClient)
        {
            MessagingCenter.Subscribe<BasePage,object>(this,Constants.LAYOUT_APPEARED,(sender,arg)=>
            {
                if(!(sender.BindingContext.Equals(this)))
                    return;
                
                OnLayouAppeared();
            });
            
            Navigation = navigation;
            DownloadService = downloadService;
            SecureStorageService = secureStorageService;
            FileService = fileService;
            ApiClient = apiClient;
            
            _downloadQueue = new Queue<SongItemViewModel>();

            #region Base Fields

            #endregion
        }

        async void OpenPlayer()
        {
            Navigation.ModalTo("PlayerPage");
        }

        protected virtual void OnLayouAppeared()
        {
            
        }

        #region Download methods

        public void DownloadUrl(SongItemViewModel song)
        {
            if (IsDownloading)
            {
                _downloadQueue.Enqueue(song);
                return;
            }

            StartDownloadAsync(song.Title, song.Url, async (filePath) =>
            {
                SecureStorageService.SaveSongToLocalStorate(song, filePath);

                if (_downloadQueue.Count <= 0)
                    return;

                var nextSong = _downloadQueue.Dequeue();

                if (nextSong == null)
                    return;

                await Task.Delay(300);

                DownloadUrl(nextSong);
            });
        }
        
        public async Task<SongItemViewModel> GetSongUrl(SongItemViewModel song)
        {
            StaticUI.Instance.StartLoading();

            var medias = await ApiClient.GetMediaItems(song.Id);

            StaticUI.Instance.StopLoading();

            if (medias != null)
            {
                try
                {
                    song.Url = medias.GetAudioUrl();

                    return song;
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                    return null;
                }
            }
            else
                return null;
        }

        public async Task StartDownloadAsync(string songName, string url, Action<string> onCompleted)
        {
            if (IsDownloading)
                return;

            DownloadingSongName = songName;
            var progressIndicator = new Progress<double>(ReportProgress);
            var cts = new CancellationTokenSource();
            try
            {
                IsDownloading = true;

                await DownloadService.DownloadFileAsync(url, progressIndicator, (filePath) =>
                {
                    MainThread.BeginInvokeOnMainThread(() =>
                    {
                        IsDownloading = false;
                        onCompleted?.Invoke(filePath);
                    });
                }, cts.Token,  Device.RuntimePlatform == Device.Android ? "webm" : "mp4");
            }
            catch (OperationCanceledException ex)
            {
                System.Diagnostics.Debug.WriteLine(ex.ToString());
                IsDownloading = false;
            }
        }

        internal void ReportProgress(double value)
        {
            ProgressValue = value;
        }

        #endregion
    }
}