using System;
using System.Globalization;
using System.Text.RegularExpressions;

namespace MhLabs.Calendar
{
    public static class Calendar
    {
        private static readonly Regex _dateFormat = new Regex(@"^\d{4}-((0[1-9])|(1[012]))-((0[1-9]|[12]\d)|3[01])*");

        public static DateTime Now(string timeZone)
        {
            var timeZoneInfo = TimeZoneKeeper.GetTimeZone(timeZone);
            var result = TimeZoneInfo.ConvertTime(DateTime.UtcNow, timeZoneInfo);

            return result;
        }

        public static DateTime ParseAsLiteral(string dateTime)
        {
            if (string.IsNullOrWhiteSpace(dateTime) || !_dateFormat.IsMatch(dateTime))
            {
                throw new ArgumentException($"Not a valid date in YYYY-MM-DD format: {dateTime}", nameof(dateTime));
            }

            var year = dateTime.Substring(0, 4);
            var month = dateTime.Substring(5, 2);
            var day = dateTime.Substring(8, 2);
            var hour = 0;
            var minute = 0;
            var second = 0;

            if (dateTime.Length >= 18 && TimeSpan.TryParse(dateTime.Substring(11, 8), out var time))
            {
                hour = time.Hours;
                minute = time.Minutes;
                second = time.Seconds;
            }

            var result = new DateTime(int.Parse(year), int.Parse(month), int.Parse(day),
                hour, minute, second, DateTimeKind.Unspecified);

            return result;
        }

        public static DateTime ConvertFromOffset(string dateTime, string destinationTimeZone)
        {
            if (!DateTimeOffset.TryParse(dateTime, out var parsed))
            {
                throw new ArgumentException($"Not a valid offset value: {dateTime}", nameof(dateTime));
            }

            var result = TimeZoneInfo.ConvertTime(parsed, TimeZoneKeeper.GetTimeZone(destinationTimeZone));
            return result.DateTime;
        }

        public static int GetWeekOfYear(DateTime date)
        {
            var thursday = date.AddDays(3 - (((int)date.DayOfWeek + 6) % 7));
            var week = 1 + ((thursday.DayOfYear - 1) / 7);

            return week;
        }
    }
}