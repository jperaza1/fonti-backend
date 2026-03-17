using Microsoft.AspNetCore.Http;
using cod.application.Interfaces;
using cod.infraestructure.Implementations;
using cod.persistence.Implementations;
using StructureMap;

namespace cod.auth.DependencyInjection
{
    public class StructureMapRegistry : Registry
    {
        public StructureMapRegistry()
        {

            For<IRepository>()
                .Use<Repository>();

            For<IEmailService>()
               .Use<EmailService>();

            For<IHttpContextAccessor>()
                .Use<HttpContextAccessor>();

            For<IConfigurationService>()
                .Use<ConfigurationService>();
        }
    }
}
