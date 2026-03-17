using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;

namespace cod.application.EnumDefinition.Queries.GetPersonType
{
    public interface IGetPersonTypeQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
