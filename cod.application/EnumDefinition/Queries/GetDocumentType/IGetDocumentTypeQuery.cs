using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetDocumentType
{
    public interface IGetDocumentTypeQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
