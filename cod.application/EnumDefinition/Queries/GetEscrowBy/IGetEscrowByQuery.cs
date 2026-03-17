using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetEscrowBy
{
    public interface IGetEscrowByQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}