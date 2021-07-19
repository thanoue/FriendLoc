using System;
using Newtonsoft.Json;

namespace iCho.Core.ApiModel
{
    public class TokenApiModel
    {
        [JsonProperty("token")]
        public string Token { get; set; }

        [JsonProperty("type")]
        public string Type { get; set; }

        public TokenApiModel()
        {
        }
    }
}
