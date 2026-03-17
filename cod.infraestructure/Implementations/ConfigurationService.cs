using System.ComponentModel;
using System.Threading.Tasks;
using cod.application.Features.Exception;
using cod.application.Features.Extensions;
using cod.application.Interfaces;
using cod.domain.General;

namespace cod.infraestructure.Implementations
{
    public class ConfigurationService : IConfigurationService
    {
        private readonly IRepository _repository;

        public ConfigurationService(IRepository repository)
        {
            _repository = repository;
        }
        public async Task<T> Get<T>(string key)
        {
            _repository.CustomTenantId = true;
            _repository.TenantId = 3;
            Configuration configuration = await _repository
                .FirstOrDefaultActive<Configuration>(config => config.Key.ToUpper() == key.ToUpper());

            if (configuration.IsNull())
                throw new ApplicationException($"Please configure {key}", "Configurations");

            return GetType<T>(configuration.Value);

        }

        public async Task<T> Get<T>(string key, int tenantId)
        {
            _repository.DisableTenantFilter = true;
            Configuration configuration = await _repository
                .FirstOrDefaultActive<Configuration>(config => config.Key.ToUpper() == key.ToUpper() && config.TenantId == tenantId);

            if (configuration.IsNull())
                throw new ApplicationException($"Please configure {key}", "Configurations");

            _repository.DisableTenantFilter = false;

            return GetType<T>(configuration.Value);

        }

        public T GetType<T>(object customObject)
        {

            TypeConverter converter = TypeDescriptor.GetConverter(typeof(T));

            return (T)converter.ConvertFrom(customObject);

        }

        public async Task UpdateConfiguration(string key, string newValue)
        {
            Configuration configuration = await _repository
                .FirstOrDefaultActive<Configuration>(config => config.Key.ToUpper() == key.ToUpper());

            if (configuration.IsNull())
                throw new ApplicationException($"Please configure {key}", "Configurations");

            configuration.Value = newValue;

            _repository.UpdateSync(configuration);
        }
    }
}