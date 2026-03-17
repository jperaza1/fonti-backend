using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.PhoneCallType
{
    public class PhoneCallTypeQuery : IPhoneCallTypeQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;
        public PhoneCallTypeQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(domain.Features.Enums.PhoneCallType));
        }
    }
}
