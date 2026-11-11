using System.Threading.Tasks;

namespace cod.application.DispatchControl.Commands.CancelDispatchTransfer
{
    public interface ICancelDispatchTransferCommand
    {
        Task Execute(int id);
    }
}
