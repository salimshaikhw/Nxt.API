using System;

namespace Nxt.Common.Extensions
{
    public static class DateTimeExtensions
    {
        public static DateTime GetISTDateTime(this DateTime utcDateTime)
        {
            try
            {
                TimeZoneInfo ISTZone = TimeZoneInfo.FindSystemTimeZoneById("India Standard Time");
                return TimeZoneInfo.ConvertTimeFromUtc(utcDateTime, ISTZone);
            }
            catch (TimeZoneNotFoundException ex)
            {
                throw new Exception("The registry does not define the India Standard Time zone.", ex);
            }
            catch (InvalidTimeZoneException ex)
            {
                throw new Exception("Registry data on the India Standard Time zone has been corrupted.", ex);
            }
        }
    }
}
