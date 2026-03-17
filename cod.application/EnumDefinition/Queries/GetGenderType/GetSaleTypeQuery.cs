using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.EnumDefinition.Queries.GetGenderType
{
    public class GetGenderType : IGetEnumDefinitionValuesQuery<GenderType>
    {
        private readonly IEnumService _enumService;

        public GetGenderType(IEnumService enumService)
        {
            this._enumService = enumService;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _enumService.Execute<T>(typeof(GenderType));
        }
    }
}
