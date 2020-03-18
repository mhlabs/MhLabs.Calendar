using System;
using FluentAssertions;
using Xunit;

namespace MhLabs.Calendar.Tests
{
    public class CalendarTests
    {
        [Fact]
        public void Should_Convert_To_Local_Time_Zone()
        {
            var swedishTime = Calendar.Now(TimeZones.Sweden);
            swedishTime.Should().BeAfter(DateTime.UtcNow);
        }

        [Theory]
        [InlineData("2012-10-25")]
        [InlineData("2012-10-25T00:00:00+01:00")]
        [InlineData("2012-10-25T22:00:00Z")]
        public void Should_Parse_Literal_Date(string input)
        {
            var date = Calendar.ParseLiteralDate(input);

            date.Kind.Should().Be(DateTimeKind.Unspecified);
            date.Should().Be(new DateTime(2012, 10, 25));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2012")]
        [InlineData("2012-00-00")]
        [InlineData("abc")]
        public void Should_Throw_On_Invalid_Literal_Date(string input)
        {
            Assert.Throws<ArgumentException>(() => Calendar.ParseLiteralDate(input));

        }

    }
}