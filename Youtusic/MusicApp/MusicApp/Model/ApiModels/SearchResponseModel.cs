using System;
using System.Collections;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusicApp.Model.ApiModels
{
    public class SearchResponseModel
    {
        [JsonProperty("results")]
        public IList<SearchResults> Results { get; set; }

        [JsonProperty("pageInfo")]
        public SearchPageResults PageInfo { get; set; }

        public SearchResponseModel()
        {
            Results = new List<SearchResults>();
            PageInfo = new SearchPageResults();
        }
    }

    public class SearchResults
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("link")]
        public string Link { get; set; }

        [JsonProperty("kind")]
        public string Kind { get; set; }

        [JsonProperty("publishedAt")]
        public string PublishedAt { get; set; }

        [JsonProperty("channelTitle")]
        public string ChannelTitle { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("description")]
        public string Description { get; set; }

        [JsonProperty("thumbnails")]
        public SearchResultThumbnails Thumbnails { get; set; }

        public SearchResults()
        {
            Thumbnails = new SearchResultThumbnails();
        }
    }

    public class SearchResultThumbnails
    {
        [JsonProperty("Default")]
        public SearchThumbnail Default { get; set; }

        [JsonProperty("medium")]
        public SearchThumbnail Medium { get; set; }

        [JsonProperty("high")]
        public SearchThumbnail High { get; set; }

        [JsonProperty("standard")]
        public SearchThumbnail Standard { get; set; }

        [JsonProperty("maxres")]
        public SearchThumbnail Maxres { get; set; }

        public SearchResultThumbnails()
        {
            Default = new SearchThumbnail();
            Medium = new SearchThumbnail();
            High = new SearchThumbnail();
            Standard = new SearchThumbnail();
            Maxres = new SearchThumbnail();
        }
    }

    public class SearchThumbnail
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }

    public class SearchPageResults
    {
        [JsonProperty("totalResults")]
        public int TotalResults { get; set; }

        [JsonProperty("resultsPerPage")]
        public int ResultsPerPage { get; set; }

        [JsonProperty("nextPageToken")]
        public string NextPageToken { get; set; }

        [JsonProperty("prevPageToken")]
        public string PrevPageToken { get; set; }
    }
}
