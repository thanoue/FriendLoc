using System;
using Newtonsoft.Json;

namespace MusicApp.Model.ApiModels
{
    public class SearchApiModel
    {
        [JsonProperty("maxResults")]
        public int MaxResults { get; set; }

        [JsonProperty("terms")]
        public string Terms { get; set; }

        [JsonProperty("pageToken")]
        public string PageToken { get; set; }

        [JsonProperty("channelId")]
        public string ChannelId { get; set; }

        public SearchApiModel()
        {
        }
    }
}
