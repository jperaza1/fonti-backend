using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Services
{
    public class EnumService : IEnumService
    {
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>(System.Type enumType)
        {
            FieldInfo[] fields = enumType.GetFields(BindingFlags.Public | BindingFlags.Static);
            return fields.Select(field =>
            {
                var text = "";
                DescriptionAttribute[] attributes = field.GetCustomAttributes(typeof(DescriptionAttribute), false) as DescriptionAttribute[];
                if (attributes != null && attributes.Any())
                {
                    text = attributes.First().Description;
                }
                else
                {
                    text = field.Name;
                }

                return new GetEnumDefinitionValuesModel<T>()
                {
                    Text = text,
                    Value = (T)Convert.ChangeType(field.GetValue(null), typeof(T))
                };

            }).ToList();
        }
    }
}
