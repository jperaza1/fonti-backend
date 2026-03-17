using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetCarrierType
{
    public class LeadCustomerType : ILeadCustomerType
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public LeadCustomerType(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(domain.Features.Enums.LeadCustomerType));
        }
    }
}
