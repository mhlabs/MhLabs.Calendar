# MhLabs.Calendar

## Current Time

Specify one of the constants in TimeZones to get the current time.

E.g.

```C#
var now = Calendar.Now(TimeZones.Sweden);
```

## Parse date string without conversions

This is useful if you just want a date without considering time or conversions.

This means if you send in "2020-03-31" you will always get a DateTime with Kind set to unspecified and the value being 2020-03-31, regarrdless of which time zone the code runs in.

Expected format is YYYY-MM-DD, any offset info or other info afterwards is ignored.

E.g.

```C#
var date = Calendar.ParseLiteralDate("2020-03-31");
```
