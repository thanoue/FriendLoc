using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using GalaSoft.MvvmLight.Ioc;
using GalaSoft.MvvmLight.Views;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.Services.Impl;
using MusicApp.ViewModel;
using Xamarin.Forms;
using XF.Material.Forms.UI;

namespace MusicApp.Static
{
    public class Bootstrap
    {
        private static Bootstrap instance;

        public static Bootstrap Instance
        {
            get
            {
                if (instance == null)
                    instance = new Bootstrap();

                return instance;
            }
        }

        public ICustomNavigationService Setup()
        {
            SimpleIoc.Default.Register<IDownloadService, DownloadService>();
            SimpleIoc.Default.Register<IApiClient, ApiClient>();
            SimpleIoc.Default.Register<SearchViewModel>();
            SimpleIoc.Default.Register<PlayerViewModel>();
            SimpleIoc.Default.Register<HomeViewModel>();
            SimpleIoc.Default.Register<PlaylistViewModel>();
            SimpleIoc.Default.Register<ICustomNavigationService, NavigationService>();

            var navigationService = SimpleIoc.Default.GetInstance<ICustomNavigationService>();

            if (navigationService != null)
            {
                navigationService.Configure("HomePage", typeof(HomePage));
                navigationService.Configure("PlayerPage", typeof(PlayerPage));
                navigationService.Configure("SearchPage", typeof(SearchPage));
                navigationService.Configure("HomePage", typeof(HomePage));
                navigationService.Configure("PlaylistPage", typeof(PlaylistPage));
            }

            return navigationService;
        }
    }

    public interface ICustomNavigationService : INavigationService
    {
        void Configure(string pageKey, Type pageType);
        void Initialize(MaterialNavigationPage navigation);
        void ModalTo(string pageKey);
        void DismissModal();
    }

    public class NavigationService : ICustomNavigationService
    {
        private readonly Dictionary<string, Type> _pagesByKey = new Dictionary<string, Type>();
        private MaterialNavigationPage _navigation;

        public MaterialNavigationPage Navigation
        {
            get { return _navigation; }
        }

        public string CurrentPageKey
        {
            get
            {
                lock (_pagesByKey)
                {
                    if (_navigation.CurrentPage == null)
                    {
                        return null;
                    }

                    var pageType = _navigation.CurrentPage.GetType();

                    return _pagesByKey.ContainsValue(pageType)
                        ? _pagesByKey.First(p => p.Value == pageType).Key
                        : null;
                }
            }
        }

        public void GoBack()
        {
            _navigation.PopAsync(true);
            MessagingCenter.Send<INavigationService>(this, "NAVIGATING");
        }

        public void NavigateTo(string pageKey)
        {
            NavigateTo(pageKey, null);
            MessagingCenter.Send<INavigationService>(this, "NAVIGATING");
        }

        public void ModalTo(string pageKey)
        {
            PushAsModal(pageKey, null);
            MessagingCenter.Send<INavigationService>(this, "MODALING");
        }
        
        public void DismissModal()
        {
            _navigation.Navigation.PopModalAsync();
            MessagingCenter.Send<INavigationService>(this, "MODALING");
        }

        public void NavigateTo(string pageKey, object parameter)
        {
            var page = GetPage(pageKey, parameter);
            if (page != null)
                _navigation.PushAsync(page, true);
        }

        public void PushAsModal(string pageKey, object parameter)
        {
            var page = GetPage(pageKey, parameter);
            if (page != null)
                _navigation.Navigation.PushModalAsync(page, true);
        }

        Page GetPage(string pageKey, object parameter)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    var type = _pagesByKey[pageKey];
                    ConstructorInfo constructor;
                    object[] parameters;

                    if (parameter == null)
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(c => !c.GetParameters().Any());

                        parameters = new object[]
                        {
                        };
                    }
                    else
                    {
                        constructor = type.GetTypeInfo()
                            .DeclaredConstructors
                            .FirstOrDefault(
                                c =>
                                {
                                    var p = c.GetParameters();
                                    return p.Count() == 1
                                           && p[0].ParameterType == parameter.GetType();
                                });

                        parameters = new[]
                        {
                            parameter
                        };
                    }

                    if (constructor == null)
                    {
                        throw new InvalidOperationException(
                            "No suitable constructor found for page " + pageKey);
                    }

                    var page = constructor.Invoke(parameters) as Page;

                    return page;
                }
                else
                {
                    throw new ArgumentException(
                        string.Format(
                            "No such page: {0}. Did you forget to call NavigationService.Configure?",
                            pageKey),
                        "pageKey");
                }
            }
        }

        public void Configure(string pageKey, Type pageType)
        {
            lock (_pagesByKey)
            {
                if (_pagesByKey.ContainsKey(pageKey))
                {
                    _pagesByKey[pageKey] = pageType;
                }
                else
                {
                    _pagesByKey.Add(pageKey, pageType);
                }
            }
        }

        public void Initialize(MaterialNavigationPage navigation)
        {
            _navigation = navigation;
        }
    }
}