using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace cod.application.EnumDefinition.Queries.GetPayBy
{
    public class GetPayByQuery : IGetPayByQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetPayByQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.PayByOption));
        }
    }
}
