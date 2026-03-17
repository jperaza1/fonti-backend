using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetDriverTypeOperator
{
    public class GetDriverTypeOperatorQuery : IGetDriverTypeOperatorQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetDriverTypeOperatorQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.DriverTypeOperator));
        }
    }
}