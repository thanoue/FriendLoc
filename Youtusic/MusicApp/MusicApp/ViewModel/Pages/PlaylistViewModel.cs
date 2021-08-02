using System;
using System.Collections.Generic;
using System.Linq;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Static;
using MusicApp.Views.Popups;

namespace MusicApp.ViewModel
{
    public class PlaylistViewModel : BaseSearchViewModel
    {
        public override string PageName => nameof(PlaylistPage);
        public override bool IsShowPlayer => true;
        private string _playlistId;

        #region Properties

        private string _playlistTitle;
        public string PlaylistTitle
        {
            get => _playlistTitle;
            set
            {
                _playlistTitle = value;
                OnPropertyChanged();
            }
        }

        private string _playlistChannelTitle;
        public string PlaylistChannelTitle
        {
            get => _playlistChannelTitle;
            set
            {
                _playlistChannelTitle = value;
                OnPropertyChanged();
            }
        }

        private string _playlistThumbnail;
        public string PlaylistThumbnail
        {
            get => _playlistThumbnail;
            set
            {
                _playlistThumbnail = value;
                OnPropertyChanged();
            }
        }

        private string _playlistDescription;
        public string PlaylistDescription
        {
            get => _playlistDescription;
            set
            {
                _playlistDescription = value;
                OnPropertyChanged();
            }
        }

        #endregion

        public PlaylistViewModel(ICustomNavigationService navigation, IDownloadService downloadService, IApiClient apiClient, ISecureStorageService secureStorageService, IFileService fileService)
            : base(navigation, downloadService, apiClient, secureStorageService, fileService)
        {

        }

        public void SetPlaylistId(string id)
        {
            _playlistId = id;
        }

        protected override void OnLayouAppeared()
        {
            base.OnLayouAppeared();

            OnSearch("");
        }

        public override async void OnSearch(string pageToken)
        {
            StaticUI.Instance.StartLoading();
            var res = await ApiClient.GetPlaylistItems(_playlistId, PageItemCount, pageToken);
            StaticUI.Instance.StopLoading();

            if (res == null)
                return;

            PlaylistDescription = res.Info.Description;
            PlaylistTitle = res.Info.Title;
            PlaylistChannelTitle = res.Info.ChannelTitle;
            PlaylistThumbnail = res.Info.Thumbnails.Maxres.Url;

            NextPageToken = res.Items.NextPageToken;
            PrevPageToken = res.Items.PrevPageToken;

            Songs.SafeClear();
            Songs.ObtainFromPlaylist(res.Items.Items, MenuItemClicked);
            SecureStorageService.CombineSongs(Songs);
        }
    }
}
