using System;
using System.Collections.Generic;
using System.Text;
using cod.application.EnumDefinition.Queries.GetEnumDefinitionValues;
using cod.application.EnumDefinition.Queries.GetUserAlertOption;
using cod.domain.Features.Enums;

namespace LOGISTIC.CONTAINER.APPLICATION.EnumDefinition.Queries
{
    public class GetUserAlertOptionQuery : IGetUserAlertOptionQuery
    {
        private readonly IGetEnumDefinitionValues _getEnumDefinitionValues;

        public GetUserAlertOptionQuery(IGetEnumDefinitionValues getEnumDefinitionValues)
        {
            _getEnumDefinitionValues = getEnumDefinitionValues;
        }


        public List<GetEnumDefinitionValuesModel<T>> Execute<T>()
        {
            return _getEnumDefinitionValues.Execute<T>(typeof(DomainEnums.UserAlertOption));

        }
    }
}
