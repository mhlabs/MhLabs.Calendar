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

        public static string ToRoundTripDate(DateTime date, string timeZone)
        {
            return ToClientFormat(date, "yyyy-MM-dd'T'00:00:00zzz", timeZone);
        }

        public static string ToRoundTripDateTime(DateTime dateTime, string timeZone)
        {
            return ToClientFormat(dateTime, "yyyy-MM-dd'T'HH:mm:sszzz", timeZone);
        }

        private static string ToClientFormat(DateTime dateTime, string format, string timeZone)
        {
            if (dateTime == DateTime.MinValue) return string.Empty;

            var invariant = new DateTime(dateTime.Year, dateTime.Month, dateTime.Day,
                dateTime.Hour, dateTime.Minute, dateTime.Second,
                DateTimeKind.Utc);

            var zone = TimeZoneKeeper.GetTimeZone(timeZone);
            var offsetSpan = zone.GetUtcOffset(invariant);

            var offset = new DateTimeOffset(invariant.Ticks, offsetSpan);

            var result = offset.ToString(format);
            return result;
        }

        /// <summary>
        /// <para>Convert dateTime string to UTC using specified time zone</para>
        /// <para>The following example will convert Swedish datetime: 2020-06-30 15:35 to Universal datetime: 2020-06-30 13:35</para>
        /// <para></para>
        /// <code>
        ///   Calendar.ConvertToUniversalTime("2020-06-30 15:35", TimeZones.Sweden);
        /// </code>
        /// </summary>
        /// <param name="dateTime"></param>
        /// <param name="timeZone"></param>
        /// <example>Hello world: <code>var x = y;</code></example>
        public static DateTime ConvertToUniversalTime(string dateTime, string timeZone)
        {
            var parsedDateTime = ParseAsLiteral(dateTime);
            var clientFormat = ToRoundTripDateTime(parsedDateTime, timeZone);

            if (!DateTimeOffset.TryParse(clientFormat, out DateTimeOffset result))
            {
                throw new ArgumentException($"Not a valid dateTime value: {dateTime}", nameof(dateTime));
            }

            return result.UtcDateTime;
        }
    }
}