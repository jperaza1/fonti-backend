using System;
using System.Collections.Generic;
using System.Text;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;

namespace cod.application.EnumDefinition.Queries.GetIdentificationType
{
    public interface IGetIdentificationTypeQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
