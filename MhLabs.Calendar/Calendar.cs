using System;
using System.Text.RegularExpressions;

namespace MhLabs.Calendar
{
    public static class Calendar
    {
        private static readonly Regex _dateFormat = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])*");
        private static readonly TimeZoneKeeper _timeZoneKeeper;
        static Calendar()
        {
            _timeZoneKeeper = new TimeZoneKeeper();
        }

        public static DateTime Now(string timeZone)
        {
            var timeZoneInfo = _timeZoneKeeper[timeZone];
            var result = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);

            return result;
        }

        public static DateTime ParseLiteralDate(string date)
        {
            if (string.IsNullOrWhiteSpace(date) || !_dateFormat.IsMatch(date))
            {
                throw new ArgumentException("Not a valid date in YYYY-MM-DD format", nameof(date));
            }

            var year = date.Substring(0, 4);
            var month = date.Substring(5, 2);
            var day = date.Substring(8, 2);

            var result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day), 0, 0, 0, DateTimeKind.Unspecified);
            return result;
        }

    }
}