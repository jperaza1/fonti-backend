using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetPayBy
{
    public interface IGetPayByQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();

    }
}