using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.Features.Enums;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetPaymentFilterDate
{
    public class GetPaymentFilterDateQuery : IGetPaymentFilterDateQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetPaymentFilterDateQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(ApplicationEnums.PaymentFilterCriteria));
        }
    }
}
