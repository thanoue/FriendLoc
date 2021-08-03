using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using MusicApp.Model.ApiModels;
using MusicApp.Pages;
using MusicApp.Services;
using MusicApp.ViewModel;
using Plugin.Iconize;
using Xamarin.Essentials;
using Xamarin.Forms;
using Xamarin.Forms.Xaml;

namespace MusicApp.Static
{
    public static class Utils
    {
        public static Uri GetUriFromString(this string urlString)
        {
            Uri uri;
            if (Uri.TryCreate(urlString, UriKind.Absolute, out uri)
                && (uri.Scheme == Uri.UriSchemeHttp
                    || uri.Scheme == Uri.UriSchemeHttps
                    || uri.Scheme == Uri.UriSchemeFtp
                    || uri.Scheme == Uri.UriSchemeMailto
                    || uri.Scheme == Uri.UriSchemeFile))
                return uri;

            return null;
        }

        public static string GetAudioUrl(this SelectResponseModel model)
        {
            if (Device.RuntimePlatform == Device.Android)
            {
                var audios = model?.Audios?.OrderBy(p => p.AudioBitrate).Distinct().ToList();

                if (audios == null)
                    return "";

                var url = audios[0].Url;

                return url;
            }
            else
            {
                var videos = model?.Videos?.OrderBy(p => p.Bitrate).Distinct().ToList();

                if (videos == null)
                    return "";

                var url = videos.FirstOrDefault(p => p.Container == ("mp4"))?.Url ?? "";

                return url;
            }

        }

        public static ImageSource GetImageFromResource(this string imageName)
        {
            if (imageName.Contains("/"))
            {
                var parts = imageName.Split('/');
                imageName = parts[parts.Length - 1];
            }

            return ImageSource.FromResource("MusicApp.Images." + imageName, typeof(Utils).Assembly);
        }

        public static UrlModel GetId(string url)
        {
            if (url.Contains("playlist"))
            {
                var parts = url.Split('=');

                return new UrlModel()
                {
                    Id = parts[parts.Length - 1],
                    Type = UrlModel.IdTypes.Playlist
                };
            }
            else
            {
                if (url.Contains("youtu.be"))
                {
                    var parts = url.Split('/');

                    return new UrlModel()
                    {
                        Id = parts[parts.Length - 1],
                        Type = UrlModel.IdTypes.Video
                    };
                }

                return null;
            }
        }


        public static void CombineSongs(this ISecureStorageService secureStorageService, ObservableCollection<SongItemViewModel> songs)
        {
            var ids = secureStorageService.GetObject<List<string>>(Constants.SAVED_SONG_IDS);

            if (ids == null || !ids.Any())
                return;

            foreach (var song in songs)
            {
                if (ids.Contains(song.Id))
                {
                    var saved = secureStorageService.GetObject<SongItemViewModel>(song.Id);
                    if (saved == null)
                        continue;
                    song.Url = saved.Url;
                    song.Type = SongTypes.Offline;
                }
            }
        }

        public static SongItemViewModel SongIsDownloaded(this ISecureStorageService secureStorageService, string songId)
        {
            var song = secureStorageService.GetObject<SongItemViewModel>(songId);

            if (song == null || song.Id != songId)
                return null;

            return song;
        }

        public static SafeObservableCollection<SongItemViewModel> GetDownloadedSongs(this ISecureStorageService secureStorageService, Action<string> onPlay)
        {
            var list = new SafeObservableCollection<SongItemViewModel>();

            var ids = secureStorageService.GetObject<List<string>>(Constants.SAVED_SONG_IDS);

            if (ids == null || !ids.Any())
                return list;

            foreach (var id in ids)
            {
                var song = secureStorageService.GetObject<SongItemViewModel>(id);
                song.OnPlay = onPlay;

                if (song != null)
                    list.Add(song);
            }

            return list;
        }

        public static void SaveSongToLocalStorate(this ISecureStorageService secureStorageService, SongItemViewModel song, string localFileName)
        {
            var ids = secureStorageService.GetObject<List<string>>(Constants.SAVED_SONG_IDS);

            if (ids == null)
                ids = new List<string>();

            if (ids.Contains(song.Id))
                throw new Exception("This song is already downloaded!!!");

            ids.Add(song.Id);

            secureStorageService.StoreObject(Constants.SAVED_SONG_IDS, ids);

            song.Type = SongTypes.Offline;
            song.Url = localFileName;

            var oldSong = secureStorageService.GetObject<SongItemViewModel>(song.Id);

            if (oldSong != null)
            {
                oldSong.Url = localFileName;
                oldSong.Type = SongTypes.Offline;
                secureStorageService.StoreObject(oldSong.Id, oldSong);
                return;
            }

            secureStorageService.StoreObject(song.Id, song);
        }

        public static void DeleteSongFromStorage(this ISecureStorageService secureStorageService, IFileService fileService, SongItemViewModel song)
        {
            var ids = secureStorageService.GetObject<List<string>>(Constants.SAVED_SONG_IDS);
            if (ids == null || !ids.Any() || !ids.Contains(song.Id))
                return;

            ids.Remove(song.Id);

            secureStorageService.StoreObject(Constants.SAVED_SONG_IDS, ids);
            secureStorageService.Remove(song.Id);

            fileService?.DeleteFile(song.Url);
        }

        public static void SafeClear<T>(this ObservableCollection<T> observableCollection)
        {
            if (!MainThread.IsMainThread)
            {
                MainThread.BeginInvokeOnMainThread(() =>
                {
                    while (observableCollection.Any())
                    {
                        observableCollection.RemoveAt(0);
                    }
                });
            }
            else
            {
                while (observableCollection.Any())
                {
                    observableCollection.RemoveAt(0);
                }
            }
        }
    }

    public class SafeObservableCollection<T> : ObservableCollection<T>
    {
        /// <summary>
        /// Normal ObservableCollection fails if you are trying to clear ObservableCollection<ObservableCollection<T>> if there is data inside
        /// this is workaround till it won't be fixed in Xamarin Forms
        /// </summary>
        protected override void ClearItems()
        {
            while (this.Items.Any())
            {
                this.Items.RemoveAt(0);
            }
        }
    }

    [ContentProperty(nameof(ImgName))]
    public class ImageResourceExtension : IMarkupExtension
    {
        public string ImgName { get; set; }

        public object ProvideValue(IServiceProvider serviceProvider)
        {
            if (ImgName == null)
            {
                return null;
            }

            return ImgName.GetImageFromResource();
        }
    }
}
