using cod.application.DispatchControl.Commom;
using System.Threading.Tasks;

namespace cod.application.DispatchControl.Commands.CreateDispatchTransfer
{
    public interface ICreateDispatchTransferCommand
    {
        Task Execute(DispatchTransferModel model);
    }
}
