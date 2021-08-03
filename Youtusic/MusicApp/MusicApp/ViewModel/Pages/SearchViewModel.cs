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
    public class SearchViewModel : BaseSearchViewModel
    {
        #region Fields

        private int _pageItemCount = 20;
        public ICommand SearchReturnCommand { get; }

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

     
        public override string PageName => nameof(SearchPage);

        #endregion

        public SearchViewModel(ICustomNavigationService navigation,IDownloadService downloadService, IApiClient apiClient, ISecureStorageService secureStorageService,IFileService fileService) 
            : base(navigation,downloadService, apiClient, secureStorageService,fileService)
        {
            NextPageToken = "";
            PrevPageToken = "";
            SearchReturnCommand = new RelayCommand(() => { OnSearch(""); });

            SearchTerm = "yeu em dai lau";
        }

        protected override void OnLayouAppeared()
        {
            base.OnLayouAppeared();

            MediaController.Instance.Play();
        }

        #region Methods

        public override async void OnSearch(string pageToken)
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
                Songs.SafeClear();
                Songs.ObtainFromSearching(res.Results, MenuItemClicked);
                SecureStorageService.CombineSongs(Songs);
            }
            else
            {
                StaticUI.Instance.StopLoading();
            }
        }

        #endregion

    }
}