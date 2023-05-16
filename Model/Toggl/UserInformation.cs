using System.Text.Json.Serialization;

namespace ImportFromTempoToToggl.Model.Toggl
{
    internal class UserInformation
    {
        [JsonPropertyName("useremail")]
        public string UserEmail { get; set; }

        [JsonPropertyName("userpassword")]
        public string UserPassword { get; set; }

        [JsonPropertyName("workspaceid")]
        public int? WorkSpaceId { get; set; }
    }
}