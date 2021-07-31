using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using MusicApp.Model.ApiModels;

namespace MusicApp.ViewModel
{
    public static class DataHelper
    {
        public static void ObtainFromSelecting(this ObservableCollection<SongItemViewModel> src,IList<RelatedVideo> relatedVideos,Action<string> onPlay)
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
        public static void ObtainFromSearching(this ObservableCollection<SongItemViewModel> src,
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
    }
}