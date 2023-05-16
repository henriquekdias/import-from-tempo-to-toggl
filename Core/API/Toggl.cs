using ImportFromTempoToToggl.Model.Toggl;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Text.Json;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace ImportFromTempoToToggl.Core.API
{
    internal class Toggl
    {
        private HttpClient _client;

        public Toggl(Model.Toggl.UserInformation togglUserInformation)
        {
            _client = new HttpClient();

            _client.BaseAddress = new Uri("https://api.track.toggl.com/api/v9/");
            _client.DefaultRequestHeaders.Accept.Clear();
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            _client.DefaultRequestHeaders.Authorization =
                new AuthenticationHeaderValue(
                    "Basic",
                    Convert.ToBase64String(
                        System.Text.Encoding.ASCII.GetBytes(
                                   $"{togglUserInformation.UserEmail}:{togglUserInformation.UserPassword}")));

        }

        public async Task<List<ImportFromTempoToToggl.Model.Toggl.TimeEntry>> GetEntries()
        {
            var timeEntry = new List<ImportFromTempoToToggl.Model.Toggl.TimeEntry>();

            //https://api.track.toggl.com/api/v9/me/time_entries
            var url = "me/time_entries";

            var response = _client.GetAsync(url).Result;

            if (response.IsSuccessStatusCode)
            {
                timeEntry = await response.Content.ReadAsAsync<List<ImportFromTempoToToggl.Model.Toggl.TimeEntry>>();
            }
            else
            {
                return null;
            }

            return timeEntry;
        }

        public async Task CreateTimeEntry(TimeEntry timeEntry, int workspaceId)
        {
            if (timeEntry == null)
                return;

            var url = $"workspaces/{workspaceId}/time_entries";

            string jsonString = JsonSerializer.Serialize(timeEntry);

            var request = new StringContent(jsonString);

            HttpResponseMessage response = null;

            try
            {
                response = _client.PostAsync(
                    url,
                    request
                ).Result;

                response.EnsureSuccessStatusCode();

                string content = await response.Content.ReadAsStringAsync();

                //Console.WriteLine(response.StatusCode);
                //Console.WriteLine(content);
            }
            catch (System.Exception e)
            {
                Console.WriteLine("=======================");
                Console.WriteLine(e.Message);

                if (response != null)
                {
                    Console.WriteLine("-----------------------");
                    Console.WriteLine(response.StatusCode);
                }

                Console.WriteLine("=======================");
                Console.WriteLine();
            }
        }
    }
}