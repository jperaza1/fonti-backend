using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace cod.application.Common.Extensions
{
    public static class StringExtension
    {
        public static string Upper(this string value)
        {
            return !string.IsNullOrEmpty(value) ? value.ToUpper() : value;
        }
        public static string TrimAll(this string value)
        {
            return Regex.Replace(value, @"\s+", "");
        }
        public static byte[] ConvertToPicture(this string value)
        {
            return string.IsNullOrEmpty(value)
                ? new byte[] { }
                : Convert.FromBase64String(value.Split(',').LastOrDefault() ?? string.Empty);
        }

        public static List<string> ExtractList(this string value)
        {
            return string.IsNullOrEmpty(value) ? new List<string>() : value.Split(';').ToList();
        }
        public static string ToTaxId(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Regex.Replace(value.Replace("-", ""), @"(\d{2})(\d{7})", "$1-$2");
        }
        public static string ToSsn(this string value)
        {
            return string.IsNullOrEmpty(value) ? string.Empty : Regex.Replace(value.Replace("-", ""), @"(\d{3})(\d{2})(\d{4})", "$1-$2-$3");
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

        public static string ToCamelCase(this string value)
        {
            return string.IsNullOrEmpty(value) || value.Length < 2
                ? value
                : char.ToLowerInvariant(value[0]) + value.Substring(1);
        }

        public static bool IsAddressValid(this string value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return char.IsNumber(value.First());
        }

        public static string CapitalLetters(this string value)
        {

            if (string.IsNullOrEmpty(value))
                return string.Empty;

            return Regex.Split(value, @"(?<!^)(?=[A-Z])")
                 .Select(splitString => splitString.FirstOrDefault().ToString()).Aggregate((a, b) => $"{a}{b}");
        }

        public static double ToDouble(this string value, int decimalPrecision)
        {
            string _integer = value.Substring(0, value.Length - decimalPrecision);
            string _float = value.Substring(value.Length - decimalPrecision);

            double _double = 0;
            var parse = double.TryParse(_integer, out _double);

            return parse ? Convert.ToDouble(_integer + "." + _float, System.Globalization.CultureInfo.InvariantCulture) : 0;
        }
    }
}
