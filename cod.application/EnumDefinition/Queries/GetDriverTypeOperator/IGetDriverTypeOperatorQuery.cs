using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Text;

namespace cod.application.EnumDefinition.Queries.GetDriverTypeOperator
{
    public interface IGetDriverTypeOperatorQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
