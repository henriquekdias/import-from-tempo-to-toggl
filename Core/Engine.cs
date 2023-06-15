using Microsoft.VisualBasic.FileIO;
using ImportFromTempoToToggl.Core.API;
using ImportFromTempoToToggl.Core.Support;
using System.Text.Json;

namespace ImportFromTempoToToggl.Core
{
    internal class Engine
    {
        internal readonly string _applicationPath = System.AppDomain.CurrentDomain.BaseDirectory;

        public void Run()
        {

            var tempoTimeEntries = GetTempoTimeEntriesFromCsv();
            if (tempoTimeEntries.Count == 0)
            {
                Console.WriteLine("Not able to get Tempo time entries from csv file");
                return;
            }

            var togglUserInformation = ReadTogglUserInformation();
            if (togglUserInformation == null)
            {
                Console.WriteLine("Not able to get User Toggl information from json");
                return;
            }

            var togglTimeEntries = ConvertTempoTimeEntriesToTogglTimeEntries(tempoTimeEntries, togglUserInformation.WorkSpaceId);
            if (togglTimeEntries.Count == 0)
            {
                 Console.WriteLine("Unable to convert Tempo time entries to Toggl time entries");
                return;
            }

            CreateTogglTimeEntries(togglTimeEntries, togglUserInformation);
        }

        private List<Model.Tempo.TimeEntry> GetTempoTimeEntriesFromCsv()
        {
            var w = new Watcher();
            w.Start("getting timeEntries from tempo.csv");

            var tempoTimeEntries = new List<Model.Tempo.TimeEntry>();

            try
            {
                var filePath = $"{_applicationPath}rawtempo.csv";

                if (!File.Exists(filePath))
                    return tempoTimeEntries;

                using (TextFieldParser csvParser = new TextFieldParser(filePath))
                {
                    csvParser.CommentTokens = new string[] { "#" };
                    csvParser.SetDelimiters(new string[] { "," });
                    csvParser.HasFieldsEnclosedInQuotes = true;

                    // Read headers row
                    string[] header = csvParser.ReadFields();

                    while (!csvParser.EndOfData)
                    {
                        // Read current line fields, pointer moves to the next line.
                        string[] fields = csvParser.ReadFields();

                        var tempoTimeEntry = new Model.Tempo.TimeEntry();
                        tempoTimeEntry.Key = fields[0];
                        tempoTimeEntry.Summary = fields[1];
                        tempoTimeEntry.Duration = fields[2];
                        tempoTimeEntry.Date = fields[3];

                        tempoTimeEntries.Add(tempoTimeEntry);
                    }
                }
            }
            catch (System.Exception e)
            {
                throw;
            }

            w.Stop();

            return tempoTimeEntries;
        }

        private Model.Toggl.UserInformation ReadTogglUserInformation()
        {
            var w = new Watcher();
            w.Start("getting toggl user information");


            var userInformation = new Model.Toggl.UserInformation();

            try
            {
                var fileName = $"{_applicationPath}toggluserinformation.json";

                if (File.Exists(fileName))
                {
                    string jsonString = File.ReadAllText(fileName);
                    userInformation = JsonSerializer.Deserialize<Model.Toggl.UserInformation>(jsonString);
                }
                else
                {

                }
            }
            catch (System.Exception e)
            {
                userInformation = null;
            }

            w.Stop();

            return userInformation;
        }

        private List<Model.Toggl.TimeEntry> ConvertTempoTimeEntriesToTogglTimeEntries(List<Model.Tempo.TimeEntry> tempoTimeEntries, int? togglWorkSpaceId)
        {
            var w = new Watcher();
            w.Start("converting tempo time entries to toggl time entries");

            var togglTimeEntries = new List<Model.Toggl.TimeEntry>();

            if (tempoTimeEntries == null)
                return togglTimeEntries;

            if (!togglWorkSpaceId.HasValue)
                return togglTimeEntries;

            var timeConverter = new TimeConverter();

            foreach (var tempoTimeEntry in tempoTimeEntries)
            {
                var togglTimeEntry = new Model.Toggl.TimeEntry();

                togglTimeEntry.WorkspaceId = togglWorkSpaceId.Value;
                togglTimeEntry.Description = $"{tempoTimeEntry.Key} - {tempoTimeEntry.Summary}";
                togglTimeEntry.Duration = timeConverter.DurationFromTempoToToggl(tempoTimeEntry.Duration);
                togglTimeEntry.Start = timeConverter.StartFromTempoToToggl(tempoTimeEntry.Date);

                togglTimeEntries.Add(togglTimeEntry);
            }

            w.Stop();

            return togglTimeEntries;
        }

        private void CreateTogglTimeEntries(List<Model.Toggl.TimeEntry> togglTimeEntries, Model.Toggl.UserInformation togglUserInformation)
        {
            var user = togglUserInformation.Email;
            if(string.IsNullOrEmpty(user))
            {
                user = togglUserInformation.Token;
            }

            var w = new Watcher();
            w.Start($"creating time entries on Toggl using {user}");

            if (togglUserInformation == null)
                return;

            if(string.IsNullOrEmpty(togglUserInformation.Token))
            {
                if (string.IsNullOrEmpty(togglUserInformation.Email))
                    return;

                if (string.IsNullOrEmpty(togglUserInformation.Password))
                    return;
            }

            if (!togglUserInformation.WorkSpaceId.HasValue)
                return;

            var togglApi = new Toggl(togglUserInformation);

            foreach (var togglTimeEntry in togglTimeEntries)
            {
                togglApi.CreateTimeEntry(togglTimeEntry, togglUserInformation.WorkSpaceId.Value).Wait();
            }

            w.Stop();
        }
    }
}