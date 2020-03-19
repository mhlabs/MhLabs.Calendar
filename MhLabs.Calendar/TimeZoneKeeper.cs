using System.Globalization;
using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TimeZoneConverter;

namespace MhLabs.Calendar
{
    public static class TimeZoneKeeper
    {
        private static readonly Dictionary<string, TimeZoneInfo> _configuredTimeZones = new Dictionary<string, TimeZoneInfo>();

        static TimeZoneKeeper()
        {
            _configuredTimeZones = new Dictionary<string, TimeZoneInfo>
            {
                { TimeZones.Sweden, TZConvert.GetTimeZoneInfo(TimeZones.Sweden)},
                { TimeZones.Utc, TZConvert.GetTimeZoneInfo(TimeZones.Utc)}
            };
        }

        public static TimeZoneInfo GetTimeZone(string timeZone)
        {
            if (_configuredTimeZones.ContainsKey(timeZone))
            {
                return _configuredTimeZones[timeZone];
            }

            throw new ArgumentException($"TimeZone is not configured: {timeZone}", nameof(timeZone));
        }
    }
}
