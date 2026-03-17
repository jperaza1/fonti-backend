using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetPaymentStatus
{
    public interface IGetPaymentStatusQuery
    {
        List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
