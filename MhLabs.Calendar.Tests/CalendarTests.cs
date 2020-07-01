using System.Globalization;
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
        [InlineData("2012-10-25T00:00:00Z")]
        public void Should_Parse_Literal_With_Date_Only(string input)
        {
            var date = Calendar.ParseAsLiteral(input);

            date.Kind.Should().Be(DateTimeKind.Unspecified);
            date.Should().Be(new DateTime(2012, 10, 25));
        }

        [Theory]
        [InlineData("2012-10-25T03:30:33+01:00")]
        [InlineData("2012-10-25 03:30:33")]
        public void Should_Parse_Literal_With_Time(string input)
        {
            var date = Calendar.ParseAsLiteral(input);

            date.Kind.Should().Be(DateTimeKind.Unspecified);
            date.Should().Be(new DateTime(2012, 10, 25, 3, 30, 33));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2012")]
        [InlineData("2012-00-00")]
        [InlineData("abc")]
        public void Should_Throw_On_Invalid_Literal(string input)
        {
            Assert.Throws<ArgumentException>(() => Calendar.ParseAsLiteral(input));

        }

        [Theory]
        [InlineData("2020-03-24T23:00:00+00:00")]
        [InlineData("2020-03-24T23:00:00Z")]
        [InlineData("2020-03-25T00:00:00+01:00")]
        public void Should_Convert_From_Offset(string input)
        {
            var date = Calendar.ConvertFromOffset(input, TimeZones.Sweden);

            date.Kind.Should().Be(DateTimeKind.Unspecified);
            date.Should().Be(new DateTime(2020, 3, 25));
        }

        [Theory]
        [InlineData("")]
        [InlineData(null)]
        [InlineData("2012")]
        [InlineData("2012-00-00T25:12:12")]
        [InlineData("2012-00-00T05:12:12+999:00:00")]
        [InlineData("abc")]
        public void Should_Throw_On_Invalid_Offset_Value(string input)
        {
            Assert.Throws<ArgumentException>(() => Calendar.ConvertFromOffset(input, TimeZones.Sweden));

        }

        [Theory]
        [InlineData("2019-12-30", 1)]
        [InlineData("2019-12-31", 1)]
        [InlineData("2020-03-19", 12)]
        [InlineData("2016-01-03", 53)]
        public void Should_Return_Correct_Week_Number(string dateString, int exepectedWeekNmber)
        {
            var date = DateTime.Parse(dateString, CultureInfo.InvariantCulture, DateTimeStyles.None);
            var week = Calendar.GetWeekOfYear(date);

            week.Should().Be(exepectedWeekNmber);
        }

        [Theory]
        [InlineData("2020-03-12T01:30:00+00:00", TimeZones.Utc)]
        [InlineData("2020-03-12T01:30:00-09:00", TimeZones.Sweden)]
        [InlineData("2020-03-12T01:30:00+00:00", TimeZones.Sweden)]
        [InlineData("2020-03-12T01:30:00+01:00", TimeZones.Sweden)]
        public void Should_Convert_To_Round_Trip_Date(string dateString, string timeZone)
        {
            var date = Calendar.ParseAsLiteral(dateString);
            var offset = timeZone == TimeZones.Utc ? "+00:00" : "+01:00";

            var clientFormat = Calendar.ToRoundTripDate(date, timeZone);

            clientFormat.Should().Be($"2020-03-12T00:00:00{offset}");
        }

        [Theory]
        [InlineData("2020-03-12T05:45:36-11:00", TimeZones.Sweden)]
        [InlineData("2020-03-12T05:45:36+01:00", TimeZones.Utc)]
        [InlineData("2020-03-12T05:45:36+00:00", TimeZones.Utc)]
        [InlineData("2020-03-12T05:45:36+01:00", TimeZones.Sweden)]
        public void Should_Convert_To_Round_Trip_Date_Time(string dateString, string timeZone)
        {
            var date = Calendar.ParseAsLiteral(dateString);
            var offset = timeZone == TimeZones.Utc ? "+00:00" : "+01:00";

            var clientFormat = Calendar.ToRoundTripDateTime(date, timeZone);

            clientFormat.Should().Be($"2020-03-12T05:45:36{offset}");
        }


        [Theory]
        [InlineData("2020-06-30 15:35:00", TimeZones.Sweden, "2020-06-30T13:35:00+00:00")]
        [InlineData("2020-06-30 15:35:00", TimeZones.Utc, "2020-06-30T15:35:00+00:00")]
        public void Should_Convert_To_UniversalTime(string dateTime, string timeZone, string expected)
        {
            // act
            var actual = Calendar.ConvertToUniversalTime(dateTime, timeZone);

            // assert
            Assert.Equal(DateTime.Parse(expected).ToUniversalTime(), actual);
        }


        [Fact]
        public void ConvertToUniversalTime_Should_Throw_An_Error_When_An_Invalid_DateTime_Is_Used()
        {
            // arrange
            var invalidDateTime = "2000";

            // act & assert
            Assert.ThrowsAny<ArgumentException>(() => Calendar.ConvertToUniversalTime(invalidDateTime, TimeZones.Sweden));
        }
    }
}