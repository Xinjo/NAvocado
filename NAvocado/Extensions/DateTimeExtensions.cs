using System;

namespace NAvocado.Extensions
{
    public static class DateTimeExtensions
    {
        /// <summary>
        ///     Converts a given <see cref="DateTime" /> into a Unix timestamp as <see cref="int" />
        /// </summary>
        /// <param name="value">Any <see cref="DateTime" /></param>
        /// <returns>The given <see cref="DateTime" /> in Unix timestamp format</returns>
        public static int ToUnixTimestampAsInt(this DateTime value)
        {
            return (int) value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Converts a given <see cref="DateTime" /> into a Unix timestamp as <see cref="long" />
        /// </summary>
        /// <param name="value">Any <see cref="DateTime" /></param>
        /// <returns>The given <see cref="DateTime" /> in Unix timestamp format</returns>
        public static long ToUnixTimestampAsLong(this DateTime value)
        {
            return (long) value.ToUniversalTime().Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Convert a Unix timestamp to <see cref="DateTime" /> format
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <param name="unixTimestamp">Unix timestamp to convert</param>
        /// <returns>Unix timestamp converted to <see cref="DateTime" /></returns>
        public static DateTime FromUnixTimestamp(this int ignored, int unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp).ToLocalTime();
        }

        /// <summary>
        ///     Convert a Unix timestamp to <see cref="DateTime" /> format
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <param name="unixTimestamp">Unix timestamp to convert</param>
        /// <returns>Unix timestamp converted to <see cref="DateTime" /></returns>
        public static DateTime FromUnixTimestamp(this long ignored, long unixTimestamp)
        {
            return new DateTime(1970, 1, 1, 0, 0, 0, 0, DateTimeKind.Utc).AddSeconds(unixTimestamp).ToLocalTime();
        }

        /// <summary>
        ///     Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp as <see cref="int" /></returns>
        public static int CurrentTimeAsUnixTimestampAsInt(this DateTime ignored)
        {
            return (int) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }

        /// <summary>
        ///     Gets a Unix timestamp representing the current moment
        /// </summary>
        /// <param name="ignored">Parameter ignored</param>
        /// <returns>Now expressed as a Unix timestamp as <see cref="long" /></returns>
        public static long CurrentTimeAsUnixTimestampAsLong(this DateTime ignored)
        {
            return (long) DateTime.UtcNow.Subtract(new DateTime(1970, 1, 1)).TotalSeconds;
        }
    }
}