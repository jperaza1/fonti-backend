using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetWeightType
{
    public interface IGetWeightTypeQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}