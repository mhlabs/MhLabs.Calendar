using System;
using Xunit;

namespace MhLabs.Calendar.Tests
{
    public class TimeZoneKeeperTests
    {
        [Fact]
        public void Should_Init_TimeZones()
        {
            var tested = new TimeZoneKeeper();

            var sweden = tested[TimeZones.Sweden];
            var utc = tested[TimeZones.Utc];

            Assert.NotNull(sweden);
            Assert.NotNull(utc);
        }

        [Fact]
        public void Should_Throw_On_Invalid_TimeZone()
        {
            var tested = new TimeZoneKeeper();

            Assert.Throws<ArgumentException>(() => tested["Åmål"]);
        }

    }
}