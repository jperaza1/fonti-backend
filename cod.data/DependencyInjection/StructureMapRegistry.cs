using Microsoft.AspNetCore.Http;
using cod.application.Interfaces;
using cod.persistence.Implementations;
using StructureMap;
using cod.application.Common.Interfaces;
using cod.application.EnumDefinition.Queries.GetPurchaseOrderStatus;
using static cod.domain.Features.Enums.DomainEnums;

namespace cod.data.DependencyInjection
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {

            For<IRepository>()
                .Use<Repository>();

            For<IHttpContextAccessor>()
                .Use<HttpContextAccessor>();
        }
    }
}
