using System;
using Newtonsoft.Json;

namespace Authentication.Firebase
{
    [Serializable]
    public class RefreshTokenResponse
    {
        [JsonProperty("id_token")] public string IdToken;
        [JsonProperty("refresh_token")] public string RefreshToken;
        [JsonProperty("expires_in")] public string ExpiresIn;
        [JsonProperty("user_id")] public string UserId;
    }
}