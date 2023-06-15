using System.Text.Json.Serialization;

namespace ImportFromTempoToToggl.Model.Toggl
{
    internal class UserInformation
    {
        [JsonPropertyName("usertoken")]
        public string? Token { get; set; }

        [JsonPropertyName("useremail")]
        public string? Email { get; set; }

        [JsonPropertyName("userpassword")]
        public string? Password { get; set; }

        [JsonPropertyName("workspaceid")]
        public int? WorkSpaceId { get; set; }
    }
}