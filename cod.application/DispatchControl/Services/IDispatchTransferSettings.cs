using System.Threading.Tasks;

namespace cod.application.DispatchControl.Services
{
    public interface IDispatchTransferSettings
    {
        Task<bool> IsEnabled();
    }
}
