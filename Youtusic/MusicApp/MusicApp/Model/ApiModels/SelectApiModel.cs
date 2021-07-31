using System;
using Newtonsoft.Json;

namespace MusicApp.Model.ApiModels
{
    public class SelectApiModel
    {
        [JsonProperty("src")]
        public string Src { get; set; }

        public SelectApiModel()
        {
        }

        public SelectApiModel(string src)
        {
            Src = src;
        }
    }
}
