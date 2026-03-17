using cod.application.Common.Attribute;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.Extensions
{
    public static class ObjectExtension
    {
        public static IEnumerable<List<T>> Partition<T>(this List<T> values, int chunkSize)
        {
            for (int i = 0; i < values.Count; i += chunkSize)
            {
                yield return values.GetRange(i, Math.Min(chunkSize, values.Count - i));
            }
        }

        public static List<T> ConcatList<T>(this List<T> value, List<T> newList)
        {
            if (newList != null
                && newList.Any())
                value.AddRange(newList);

            return value;
        }

        public static bool IsNull<T>(this T value)
        {
            return value == null;
        }

        public static List<CompareModel> Compare<T>(this T value, object remoteValue, bool validWithAttribute = false)
        {
            List<CompareModel> compareResult = new List<CompareModel>();

            var currentProperties = value.GetType().GetProperties();
            var remoteProperties = remoteValue.GetType().GetProperties();

            foreach (var currentProperty in currentProperties)
            {
                var remotePropertyWithEqualName =
                    remoteProperties.FirstOrDefault(prop => prop.Name == currentProperty.Name);

                if (remotePropertyWithEqualName == null)
                    continue;

                var oldValue = currentProperty.GetValue(value);
                var newValue = remotePropertyWithEqualName.GetValue(remoteValue);

                var displayAttribute = remotePropertyWithEqualName.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                var reportableAttribute = remotePropertyWithEqualName.GetCustomAttribute(typeof(FieldReportableAttribute)) as FieldReportableAttribute;

                bool changed = (oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue));

                if (validWithAttribute && !reportableAttribute.IsNull() && changed)
                    compareResult.Add(new CompareModel
                    {
                        Display = displayAttribute?.DisplayName ?? string.Empty,
                        Field = currentProperty.Name.ToCamelCase(),
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                else if (!validWithAttribute && changed)
                    compareResult.Add(new CompareModel
                    {
                        Display = displayAttribute?.DisplayName ?? string.Empty,
                        Field = currentProperty.Name.ToCamelCase(),
                        OldValue = oldValue,
                        NewValue = newValue
                    });

            }

            return compareResult;
        }

        public static List<CompareModel> CompareLog<T>(this T value, object remoteValue, bool validWithAttribute = false)
        {
            List<CompareModel> compareResult = new List<CompareModel>();

            var currentProperties = value.GetType().GetProperties();
            var remoteProperties = remoteValue.GetType().GetProperties();

            foreach (var currentProperty in currentProperties)
            {
                var remotePropertyWithEqualName =
                    remoteProperties.FirstOrDefault(prop => prop.Name == currentProperty.Name);

                if (remotePropertyWithEqualName == null)
                    continue;

                var oldValue = currentProperty.GetValue(value);
                var newValue = remotePropertyWithEqualName.GetValue(remoteValue);

                var displayAttribute = remotePropertyWithEqualName.GetCustomAttribute(typeof(DisplayNameAttribute)) as DisplayNameAttribute;
                var reportableAttribute = remotePropertyWithEqualName.GetCustomAttribute(typeof(FieldLogChangedAttribute)) as FieldLogChangedAttribute;

                bool changed = (oldValue == null && newValue != null) || (oldValue != null && !oldValue.Equals(newValue));

                if (validWithAttribute && !reportableAttribute.IsNull() && changed)
                    compareResult.Add(new CompareModel
                    {
                        Display = displayAttribute?.DisplayName ?? string.Empty,
                        Field = currentProperty.Name.ToCamelCase(),
                        OldValue = oldValue,
                        NewValue = newValue
                    });
                else if (!validWithAttribute && changed)
                    compareResult.Add(new CompareModel
                    {
                        Display = displayAttribute?.DisplayName ?? string.Empty,
                        Field = currentProperty.Name.ToCamelCase(),
                        OldValue = oldValue,
                        NewValue = newValue
                    });

            }

            return compareResult;
        }
    }
}
