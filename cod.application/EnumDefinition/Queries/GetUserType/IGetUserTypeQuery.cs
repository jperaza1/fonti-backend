using System.Collections.Generic;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;

namespace cod.application.EnumDefinition.Queries.GetUserType
{
    public interface IGetUserTypeQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
