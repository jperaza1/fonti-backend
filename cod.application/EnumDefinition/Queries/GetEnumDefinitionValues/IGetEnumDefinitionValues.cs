using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetEnumDefinitionValues
{
    public interface IGetEnumDefinitionValues
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>(System.Type enumType);
    }
}
