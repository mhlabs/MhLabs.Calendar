using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using TimeZoneConverter;

[assembly: InternalsVisibleTo(nameof(MhLabs.Calendar) + ".Tests")]
namespace MhLabs.Calendar
{
    public class TimeZoneKeeper
    {
        internal static Dictionary<string, TimeZoneInfo> ConfiguredTimeZones { get; private set; }

        static TimeZoneKeeper()
        {
            ConfiguredTimeZones = new Dictionary<string, TimeZoneInfo>
            {
                { TimeZones.Sweden, TZConvert.GetTimeZoneInfo(TimeZones.Sweden)},
                { TimeZones.Utc, TZConvert.GetTimeZoneInfo(TimeZones.Utc)}
            };
        }

        public TimeZoneInfo this[string timeZone]
        {
            get
            {
                if (ConfiguredTimeZones.ContainsKey(timeZone))
                {
                    return ConfiguredTimeZones[timeZone];
                }

                throw new ArgumentException($"TimeZone is not configured: {timeZone}", nameof(timeZone));
            }
        }
    }
}