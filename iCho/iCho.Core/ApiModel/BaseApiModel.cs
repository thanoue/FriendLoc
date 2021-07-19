using System;
using Newtonsoft.Json;

namespace iCho.Core.ApiModel
{
    public abstract class BaseApiModel
    {
        [JsonProperty("id")]
        public int Id { get; set; }

        public BaseApiModel()
        {
        }
    }
}
