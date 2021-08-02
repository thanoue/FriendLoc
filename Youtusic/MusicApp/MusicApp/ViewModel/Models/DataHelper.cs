using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MusicApp.Model.ApiModels;
using MusicApp.Static;

namespace MusicApp.ViewModel
{
    public static class DataHelper
    {
        public static void ObtainFromSelecting(this SafeObservableCollection<SongItemViewModel> src,IList<RelatedVideo> relatedVideos,Action<string> onPlay)
        {
            foreach (var result in relatedVideos)
            {
                src.Add(new SongItemViewModel()
                {
                    Type = SongTypes.Online,
                    Title = result.Title,
                    AuthorId = result.Author?.Id,
                    AuthorName = result.Author?.Name,
                    Description = "",
                    Id = result.Id,
                    Url = "https://www.youtube.com/watch?v="+result.Id,
                    SmallThumbnailUrl = result.Thumbnails?[0].Url,
                    BigThumbnailUrl = result.Thumbnails?[1]?.Url,
                    PublishedTimeStr = result.Published,
                    OnPlay = onPlay
                });
            }
        }

        public static void ObtainFromSearching(this SafeObservableCollection<SongItemViewModel> src,
            IList<SearchResults> results,Action<string> onPlay)
        {
            foreach (var result in results)
            {
                DateTime publishAt = DateTime.Now;

                DateTime.TryParse(result.PublishedAt, out publishAt);

                src.Add(new SongItemViewModel()
                {
                    Type = SongTypes.Online,
                    Title = result.Title,
                    AuthorId = result.ChannelId,
                    AuthorName = result.ChannelTitle,
                    Description = result.Description,
                    Id = result.Id,
                    Url = result.Link,
                    SmallThumbnailUrl = result.Thumbnails?.Default?.Url,
                    BigThumbnailUrl = result.Thumbnails?.High?.Url,
                    PublishedAt = publishAt,
                    OnPlay = onPlay
                });
            }
        }

        public static void ObtainFromPlaylist(this SafeObservableCollection<SongItemViewModel> list, IList<PlaylistVidItem> src, Action<string> onPlay)
        {
            foreach(var item in src)
            {
                list.Add(new SongItemViewModel()
                {
                    Type = SongTypes.Online,
                    Url = "https://www.youtube.com/watch?v=" + item.Snippet.ResourceId.VideoId,
                    Title = item.Snippet.Title,
                    BigThumbnailUrl = item.Snippet.Thumbnails.Maxres.Url,
                    SmallThumbnailUrl = item.Snippet.Thumbnails.Standard.Url,
                    PublishedAt = item.Snippet.PublishedAt,
                    OnPlay = onPlay,
                    AuthorId = item.Snippet.VideoOwnerChannelId,
                    AuthorName = item.Snippet.VideoOwnerChannelTitle,
                    Id = item.Snippet.ResourceId.VideoId,
                });
            }
        }
    }
}