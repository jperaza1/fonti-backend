using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.domain.Features.Enums;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetDocumentType
{
    public class GetDocumentTypeQuery : IGetDocumentTypeQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetDocumentTypeQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.DocumentTypeOption));
        }
    }
}