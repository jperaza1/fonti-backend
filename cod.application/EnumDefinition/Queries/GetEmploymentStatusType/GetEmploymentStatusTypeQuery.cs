using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.EnumDefinition.Queries.GetEmploymentStatusType
{
    public class GetEmploymentStatusTypeQuery : IGetEnumDefinitionValuesQuery<EmploymentStatusType>
    {
        private readonly IEnumService _enumService;

        public GetEmploymentStatusTypeQuery(IEnumService enumService)
        {
            _enumService = enumService;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _enumService.Execute<T>(typeof(EmploymentStatusType));
        }
    }
}
