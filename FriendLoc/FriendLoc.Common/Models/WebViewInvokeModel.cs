using System;
using Newtonsoft.Json;

namespace FriendLoc.Model
{
    public class WebViewInvokeModel
    {
        [JsonProperty("command")]
        public string Command { get; set; }

        [JsonProperty("data")]
        public string Data { get; set; }
        public WebViewInvokeModel()
        {
        }
    }
}
