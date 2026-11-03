using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Services;
using cod.domain.Features.Enums;
using System.Collections.Generic;

namespace cod.application.EnumDefinition.Queries.GetKardexMovementType
{
    public class GetKardexMovementTypeQuery : IGetEnumDefinitionValuesQuery<KardexMovementType>
    {
        private readonly IEnumService _enumService;

        public GetKardexMovementTypeQuery(IEnumService enumService)
        {
            _enumService = enumService;
        }

        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _enumService.Execute<T>(typeof(KardexMovementType));
        }
    }
}
