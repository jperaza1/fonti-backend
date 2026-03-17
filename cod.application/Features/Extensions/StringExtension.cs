using System;
using System.Linq;
using System.Text.RegularExpressions;
using cod.domain.Features;

namespace cod.application.Features.Extensions
{
    
    public static class StringExtension
    {
        public static string BoolToString(this bool value)
        {
            return value ? "YES" : "NO";
        }
        public static string ToSsn(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Regex.Replace(value.Replace("-", ""), @"(\d{3})(\d{2})(\d{4})", "$1-$2-$3");
        }

        public static string BoolToString(this bool? value)
        {
            return value.HasValue && value.Value ? "YES" : "NO";
        }
        public static byte[] ConvertToPicture(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? new byte[] { }
                : Convert.FromBase64String(value.Split(',').LastOrDefault() ?? string.Empty);
        }

        public static bool NullOrEmpty(this string value)
        {
            return string.IsNullOrEmpty(value);
        }

        public static string ToPhoneFormat(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Regex.Replace(value, @"(\d{3})(\d{3})(\d{4})", "($1)-$2-$3");
        }

        public static DateTime ToDate(this string date)
        {
            var year = Convert.ToInt32(date.Substring(0, 4));
            var month = Convert.ToInt32(date.Substring(4, 2));
            var day = Convert.ToInt32(date.Substring(6, 2));
            return new DateTime(year, month, day, 12, 0, 0);
        }
        public static DateTime ToDateTimeFromShortStr(this string date)
        {
            var year = Convert.ToInt32(date.Substring(0, 2)) + 2000;
            var month = Convert.ToInt32(date.Substring(2, 2));
            var day = Convert.ToInt32(date.Substring(4, 2));
            var hour = Convert.ToInt32(date.Substring(6, 2));
            var min = Convert.ToInt32(date.Substring(8, 2));
            return new DateTime(year, month, day, hour, min, 0);
        }
        public static DateTime ToDateTime(this string date)
        {
            var year = Convert.ToInt32(date.Substring(0, 4));
            var month = Convert.ToInt32(date.Substring(4, 2));
            var day = Convert.ToInt32(date.Substring(6, 2));
            var hour = Convert.ToInt32(date.Substring(8, 2));
            var min = Convert.ToInt32(date.Substring(10, 2));
            return new DateTime(year > Clock.Now.Year ? 1900 : year, year > Clock.Now.Year ? 01 : month, year > Clock.Now.Year ? 01 : day, hour >= 24 ? 0 : hour, min >= 60 ? 0 : min, 0);
        }
        public static DateTime? ToDateTimeAllowNull(this string date)
        {
            var year = Convert.ToInt32(date.Substring(0, 4));
            var month = Convert.ToInt32(date.Substring(4, 2));
            var day = Convert.ToInt32(date.Substring(6, 2));
            var hour = Convert.ToInt32(date.Substring(8, 2));
            var min = Convert.ToInt32(date.Substring(10, 2));

            return year > Clock.Now.AddYears(10).Year || year < Clock.Now.Year ? null : (DateTime?)new DateTime(year, month, day, hour >= 24 ? 0 : hour, min >= 60 ? 0 : min, 0);
        }
        public static string CapitalLetters(this string value)
        {

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Regex.Split(value, @"(?<!^)(?=[A-Z])")
                .Select(splitString => splitString.FirstOrDefault().ToString()).Aggregate((a, b) => $"{a}{b}");
        }
    }
}
