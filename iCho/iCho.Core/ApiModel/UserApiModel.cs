using System;
using Newtonsoft.Json;

namespace iCho.Core.ApiModel
{
    public class UserApiModel: BaseApiModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("avtUrl")]
        public string AvtUrl { get; set; }

        [JsonProperty("loginName")]
        public string LoginName { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonProperty("facebookId")]
        public string FacebookId { get; set; }

        [JsonProperty("googleId")]
        public string GoogleId { get; set; }

        public UserApiModel()
        {
        }
    }
}
