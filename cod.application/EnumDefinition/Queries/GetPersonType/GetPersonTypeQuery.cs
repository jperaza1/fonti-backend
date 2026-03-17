using System.Collections.Generic;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;

namespace cod.application.EnumDefinition.Queries.GetPersonType
{
    public class GetPersonTypeQuery : IGetPersonTypeQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetPersonTypeQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.PersonType));
        }
    }
}