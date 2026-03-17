using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace cod.application.EnumDefinition.Queries.GetEscrowBy
{
    public class GetEscrowByQuery : IGetEscrowByQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetEscrowByQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.EscrowByOption));

        }
    }
}
