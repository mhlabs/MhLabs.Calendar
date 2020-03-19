using System;
using Xunit;

namespace MhLabs.Calendar.Tests
{
    public class TimeZoneKeeperTests
    {
        [Fact]
        public void Should_Init_TimeZones()
        {
            var sweden = TimeZoneKeeper.GetTimeZone(TimeZones.Sweden);
            var utc = TimeZoneKeeper.GetTimeZone(TimeZones.Utc);

            Assert.NotNull(sweden);
            Assert.NotNull(utc);
        }

        [Fact]
        public void Should_Throw_On_Invalid_TimeZone()
        {
            Assert.Throws<ArgumentException>(() => TimeZoneKeeper.GetTimeZone("Åmål"));
        }
    }
}