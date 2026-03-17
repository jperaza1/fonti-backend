using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetFuelType
{
    public class GetFuelTypeQuery : IGetFuelTypeQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetFuelTypeQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.FuelType));

        }
    }
}
