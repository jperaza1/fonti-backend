using System.Threading.Tasks;

namespace cod.application.Interfaces
{
    public interface IConfigurationService
    {
        Task<T> Get<T>(string key);
        Task UpdateConfiguration(string key, string newValue);
    }
}
