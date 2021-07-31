using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace MusicApp.Model.ApiModels
{
    public class SelectResponseModel
    {
        [JsonProperty("relatedVideos")]
        public IList<RelatedVideo> RelatedVideos { get; set; }

        [JsonProperty("audios")]
        public IList<MediaFormat> Audios { get; set; }

        [JsonProperty("videos")]
        public IList<MediaFormat> Videos { get; set; }

        public SelectResponseModel()
        {
            RelatedVideos = new List<RelatedVideo>();
            Audios = new List<MediaFormat>();
            Videos = new List<MediaFormat>();
        }
    }

    public class MediaFormat
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("quality")]
        public string Quality { get; set; }

        [JsonProperty("videoCodec")]
        public string VideoCodec { get; set; }

        [JsonProperty("bitrate")]
        public double Bitrate { get; set; }

        [JsonProperty("contentLength")]
        public string ContentLength { get; set; }

        [JsonProperty("container")]
        public string Container { get; set; }

        [JsonProperty("targetDurationSec")]
        public double TargetDurationSec { get; set; }

        [JsonProperty("qualityLabel")]
        public string QualityLabel { get; set; }

        [JsonProperty("audioBitrate")]
        public int AudioBitrate { get; set; }

        [JsonProperty("audioChannels")]
        public int AudioChannels { get; set; }
    }

    public class RelatedVideo
    {
        [JsonProperty("id")]
        public string Id { get; set; }

        [JsonProperty("title")]
        public string Title { get; set; }

        [JsonProperty("published")]
        public string Published { get; set; }

        [JsonProperty("author")]
        public RelatedVidAuthor Author { get; set; }

        [JsonProperty("thumbnails")]
        public IList<RelatedVidThumbnail> Thumbnails { get; set; }

        [JsonProperty("lengthSeconds")]
        public double LengthSeconds { get; set; }

        [JsonProperty("viewCount")]
        public string ViewCount { get; set; }

        [JsonProperty("shortViewCountText")]
        public string ShortViewCountText { get; set; }

        public RelatedVideo()
        {
            Thumbnails = new List<RelatedVidThumbnail>();
            Author = new RelatedVidAuthor();
        }
    }

    public class RelatedVidAuthor
    {
        [JsonProperty("id")]
        public string Id { get; set; }
        
        [JsonProperty("name")]
        public string Name { get; set; }
        
        [JsonProperty("user")]
        public  string User { get; set; }

        [JsonProperty("channelUrl")]
        public  string ChannelUrl { get; set; }
        
        [JsonProperty("userUrl")]
        public string UserUrl { get; set; }
        
        [JsonProperty("verified")]
        public  bool Verified { get; set; }
        
        [JsonProperty("thumbnails")]
        public IList<RelatedVidThumbnail> Thumbnails { get; set; }

        public RelatedVidAuthor()
        {
            Thumbnails = new List<RelatedVidThumbnail>();
        }
    }

    public class RelatedVidThumbnail
    {
        [JsonProperty("url")]
        public string Url { get; set; }

        [JsonProperty("width")]
        public int Width { get; set; }

        [JsonProperty("height")]
        public int Height { get; set; }
    }
}
