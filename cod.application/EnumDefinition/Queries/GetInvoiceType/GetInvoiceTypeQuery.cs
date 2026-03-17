using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.application.EnumDefinition.Queries.GetInvoiceType
{
    public class GetInvoiceTypeQuery : IGetEnumDefinitionValuesQuery<InvoiceType>
    {
        private readonly IEnumService _enumService;

        public GetInvoiceTypeQuery(IEnumService enumService)
        {
            _enumService = enumService;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _enumService.Execute<T>(typeof(InvoiceType));
        }
    }
}
