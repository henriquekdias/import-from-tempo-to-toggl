using System.Globalization;

namespace ImportFromTempoToToggl.Core.Support
{
    internal class TimeConverter
    {
        public int DurationFromTempoToToggl(string tempoDuration)
        {
            //Input: "3"
            //Output: 10800

            //Input: "1.75"
            //Output: 6300

            //Input: "0.75"
            //Output: 2700

            //Input: "0,5"
            //Output: 1800

            if (string.IsNullOrEmpty(tempoDuration))
                return 0;

            if(tempoDuration.Contains(','))
            {
                tempoDuration = tempoDuration.Replace(',','.');
            }

            double dTempoDuration = 0;

            if (!Double.TryParse(tempoDuration, NumberStyles.Any, CultureInfo.InvariantCulture, out dTempoDuration))
                return 0;

            var timeSpan = TimeSpan.FromHours(dTempoDuration);

            return Convert.ToInt16(timeSpan.TotalSeconds);
        }

        public string StartFromTempoToToggl(string tempoDate)
        {
            //Input: "2023-05-01 08:00"
            //Output: "2023-05-01T08:00:00.000Z"

            //Input; "2023-05-03 13:00"
            //Output: "2023-05-03T13:45:00.000Z";

            //"2023-05-05T11:00:00.000-03:00"; With timezone

            DateTime dt;

            if (!DateTime.TryParse(tempoDate, out dt))
            {
                return null;
            }

            var startTogglFormat = dt.ToString("yyyy-MM-ddTHH:mm:00.000-03:00");

            return startTogglFormat;
        }
    }
}