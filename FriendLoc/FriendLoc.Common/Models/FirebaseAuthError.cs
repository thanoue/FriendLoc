using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace FriendLoc.Model
{
    public class FirebaseClientException
    {
        [JsonProperty("error")]
        public FirebaseAuthError Error { get; set; }

        public FirebaseClientException()
        {
            Error = new FirebaseAuthError();
        }
    }
    public class FirebaseAuthError
    {
        [JsonProperty("code")]
        public int Code { get; set; }

        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("errors")]
        public IList<FirebaseError> Errors { get; set; }

        public FirebaseAuthError()
        {
            Errors = new List<FirebaseError>();
        }
    }

    public class FirebaseError
    {
        [JsonProperty("message")]
        public string Message { get; set; }

        [JsonProperty("domain")]
        public string Domain { get; set; }

        [JsonProperty("reason")]
        public string Reason { get; set; }

    }
}
