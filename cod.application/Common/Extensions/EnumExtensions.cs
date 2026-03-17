using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.Extensions
{
    public static class EnumExtensions
    {
        public static bool IsValid<T>(this T value) where T : Enum
        {
            return Enum.IsDefined(typeof(T), value);
        }

        public static string GetEnumDescription<T>(this T value) where T : Enum
        {
            if (value == null)
            {
                return string.Empty;
            }

            FieldInfo fi = value.GetType().GetField(value.ToString());

            DescriptionAttribute[] attributes = fi.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];

            if (attributes != null && attributes.Any())
            {
                return attributes.First().Description;
            }

            return value.ToString();
        }
    }
}
