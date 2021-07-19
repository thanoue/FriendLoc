using System;
using Newtonsoft.Json;
using iCho.Core.Utils;

namespace iCho.Core.ApiModel
{
    public class UserLoginApiModel : BaseApiModel
    {
        [JsonProperty("fullName")]
        public string FullName { get; set; }

        [JsonProperty("email")]
        public string Email { get; set; }

        [JsonProperty("phoneNumber")]
        public string PhoneNumber { get; set; }

        [JsonProperty("avtUrl")]
        public string AvtUrl { get; set; }

        [JsonProperty("isNewUser")]
        public bool IsNewUser { get; set; }

        [JsonProperty("accessToken")]
        public string AccessToken { get; set; }

        [JsonProperty("loginName")]
        public string LoginName { get; set; }

        [JsonProperty("expireTimeMilisecond")]
        public double ExpireTimeMilisecond { get; set; }

        [JsonProperty("password")]
        public string Password { get; set; }

        [JsonIgnore]
        public DateTime ExpireTime
        {
            get
            {
                return ExpireTimeMilisecond.TotalMilisecondsToLocalTime();
            }
        }

        public UserLoginApiModel()
        {
        }
    }
}
