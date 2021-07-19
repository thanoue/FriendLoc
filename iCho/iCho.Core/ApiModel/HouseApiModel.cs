using System;
using Newtonsoft.Json;

namespace iCho.Core.ApiModel
{
    public class HouseApiModel: BaseApiModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("address")]
        public string Address { get; set; }

        [JsonProperty("ownerId")]
        public int OwnerId { get; set; }

        [JsonProperty("avtUrl")]
        public string AvtUrl { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        public HouseApiModel()
        {

        }
    }
}
