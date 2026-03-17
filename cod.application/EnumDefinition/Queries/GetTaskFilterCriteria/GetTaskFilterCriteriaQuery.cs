using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static cod.application.Features.Enums.ApplicationEnums;

namespace cod.application.EnumDefinition.Queries.GetTaskFilterCriteria
{
    public class GetTaskFilterCriteriaQuery  : IGetTaskFilterCriteriaQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetTaskFilterCriteriaQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }
        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(TaskFilterCriteria));
        }
    }
}
