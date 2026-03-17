using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.Common.Interfaces
{
    public interface IGetEnumDefinitionValuesQuery<T> where T : Enum
    {
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
