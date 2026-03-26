using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Services;
using cod.domain.Features.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetProductType
{
    public class GetProductTypeQuery : IGetEnumDefinitionValuesQuery<ProductType>
    {
        private readonly IEnumService _enumService;

        public GetProductTypeQuery(IEnumService enumService)
        {
            _enumService = enumService;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _enumService.Execute<T>(typeof(ProductType));
        }
    }
}
