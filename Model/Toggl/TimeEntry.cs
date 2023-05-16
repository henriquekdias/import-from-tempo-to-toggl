using System.Text.Json.Serialization;

namespace ImportFromTempoToToggl.Model.Toggl
{
    internal class TimeEntry
    {
        [JsonPropertyName("description")]
        public string Description { get; set; }

        [JsonPropertyName("duration")]
        public int Duration { get; set; }

        [JsonPropertyName("start")]
        public string Start { get; set; }

        [JsonPropertyName("wid")]
        public int WorkspaceId { get; set; }

        [JsonPropertyName("created_with")]
        public string CreatedWith { get; private set; }

        [JsonPropertyName("billable")]
        public bool Billable { get; private set; }

        public TimeEntry()
        {
            CreatedWith = "Snowball";

            Billable = false;
        }
    }
}