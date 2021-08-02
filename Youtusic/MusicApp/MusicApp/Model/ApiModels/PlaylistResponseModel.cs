using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusicApp.Model.ApiModels
{
    public class PlaylistResponse
    {
        public PlaylistInfoSnippet Info { get; set; }
        public PlaylistItems Items { get; set; }
    }

    public class PlaylistInfo
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("pageInfo")]
        public PlaylistPageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public IList<PlaylistInfoItem> Items { get; set; }

        public PlaylistInfo()
        {
            PageInfo = new PlaylistPageInfo();
            Items = new List<PlaylistInfoItem>();
        }
    }

    public class PlaylistInfoItem
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("snippet")]
        public PlaylistInfoSnippet Snippet { get; set; }

        public PlaylistInfoItem()
        {
            Snippet = new PlaylistInfoSnippet();
        }
    }

    public class PlaylistInfoSnippet
    {
        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("thumbnails")]
        public SearchResultThumbnails Thumbnails { get; set; }

        [JsonProperty("localized")]
        public PlaylistInfoLocalized Localized { get; set; }

        public PlaylistInfoSnippet()
        {
            Thumbnails = new SearchResultThumbnails();
            Localized = new PlaylistInfoLocalized();
        }
    }

    public class PlaylistInfoLocalized
    {
        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("Description")]
        public string Description { get; set; }
    }


    public class PlaylistItems
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("prevPageToken")]
        public string PrevPageToken { get; set; }

        [JsonProperty("pageInfo")]
        public PlaylistPageInfo PageInfo { get; set; }

        [JsonProperty("items")]
        public IList<PlaylistVidItem> Items { get; set; }

        public PlaylistItems()
        {
            Items = new List<PlaylistVidItem>();
        }
    }

    public class PlaylistPageInfo
    {
        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public int ResultsPerPage { get; set; }
    }

    public class PlaylistVidItem
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("etag")]
        public string Etag { get; set; }

        [JsonProperty("id")]
        public string IdOnPlaylist { get; set; }

        [JsonProperty("snippet")]
        public PlaylistItemSnippet Snippet { get; set; }
    }

    public class PlaylistItemSnippet
    {
        [JsonProperty("publishedAt")]
        public DateTime PublishedAt { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("playlistId")]
        public string PlaylistId { get; set; }

        [JsonProperty("position")]
        public int Position { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("videoOwnerChannelTitle")]
        public string VideoOwnerChannelTitle { get; set; }

        [JsonProperty("videoOwnerChannelId")]
        public string VideoOwnerChannelId { get; set; }

        [JsonProperty("thumbnails")]
        public SearchResultThumbnails Thumbnails { get; set; }

        [JsonProperty("resourceId")]
        public PlaylistItemResId ResourceId { get; set; }
    }

    public class PlaylistItemResId
    {
        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("videoId")]
        public string VideoId { get; set; }
    }
}
