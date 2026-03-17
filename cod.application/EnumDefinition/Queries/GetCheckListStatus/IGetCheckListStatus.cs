using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace cod.application.EnumDefinition.Queries.GetCheckListStatus
{
    public interface IGetCheckListStatus
    {
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>();
    }
}
