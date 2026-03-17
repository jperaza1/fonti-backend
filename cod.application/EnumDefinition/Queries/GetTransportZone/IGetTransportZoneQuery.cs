using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetTransportZone
{
    public interface IGetTransportZoneQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
