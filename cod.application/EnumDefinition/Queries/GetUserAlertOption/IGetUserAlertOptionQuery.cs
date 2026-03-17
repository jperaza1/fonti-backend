using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetUserAlertOption
{
    public interface IGetUserAlertOptionQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}