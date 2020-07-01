# MhLabs.Calendar

## Common use cases

### Current time with specified time zone

If you write code in one time zone and run it in another, this is often not as trivial as it sounds.

This will get the current time in specified time zone wherever code is run:

```C#
var now = Calendar.Now(TimeZones.Sweden);
```

### Parse value without any interpretation of time zone info

E.g. if you have a DateTime value for the local time where your users are but run the code in Ireland you want to make sure the DateTime value is not assumed to be UTC. (Of course, if the value includes offset info, use DateTimeOffset.Parse instead.)

```C#
var date = Calendar.ParseAsLiteral("2020-03-31");
```

### Format DateTime in round-trip format

Use `Calendar.ToRoundTripDate` or `Calendar.ToRoundTripDateTime` to convert DateTime to a round-trip string suitable for databases, API responses etc. The correct offset value will be added for the time zone you specify.

## Method reference

### Current Time

Specify one of the constants in TimeZones to get the current time.

E.g.

```C#
var now = Calendar.Now(TimeZones.Sweden);
```

### Parse date string without conversions

This is useful if you just want a date without considering time or conversions.

This means if you send in "2020-03-31" you will always get a DateTime with Kind set to unspecified and the value being 2020-03-31, regardless of which time zone the code runs in.

Expected format is "YYYY-MM-DD", "YYYY-MM-DDThh:mm:ss" or "YYYY-MM-DD hh:mm:ss". Any offset info or other info afterwards is ignored.

E.g.

```C#
var date = Calendar.ParseAsLiteral("2020-03-31");
var dateTime = Calendar.ParseAsLiteral("2020-03-31 01:20:55");
```

### Convert offset value to another time zone

E.g. if you have an offset value in UTC and want to convert it to Swedish time.

```C#
var swedishTime = Calendar.ConvertFromOffset("2020-03-24T23:00:00+00:00", TimeZones.Sweden);
// -> 2020-03-25 00:00:00
```

### Get week number for date

According to ISO/Scandinavian standard with first 4-day week.

```C#
var week = Calendar.GetWeekOfYear("2019-12-30");
// -> 1, note that .NET Framework Calendar will return 53 regardless of parameters which is incorrect
```

### Format as round-trip with or without time

Useful for storing in databases, sending to API clients etc.

Note that no conversion is made regarding any included offset, the DateTime value is treated as a literal value and the formatting only means that the correct offset suffix is added, not that date or time is changed.

#### Date only

```C#
var date = new DateTime(2020, 03, 12, 5, 45, 36);
var clientFormat = Calendar.ToRoundTripDate(date, TimeZones.Sweden);
// -> 2020-03-12T00:00:00+01:00
```

#### Date and time

```C#
var dateTime = new DateTime(2020, 03, 12, 5, 45, 36);
var clientFormat = Calendar.ToRoundTripDateTime(dateTime, TimeZones.Sweden);
// -> 2020-03-12T05:45:36+01:00
```

### Convert to universal time

Useful when you have a swedish datetime in the format of *yyyy-MM-dd HH:mm:ss* and want to convert it to UTC
```C#
var dateTime = "2020-06-30 15:35:00";
var clientFormat = Calendar.ConvertToUniversalTime(dateTime, TimeZones.Sweden);
// -> 2020-03-12 13:35:00
```